namespace KodaEWallet.Models;

public sealed class Transaction
{
	
	private Transaction() { }
	public static Transaction CreateTopUp(Wallet receiver, decimal amount, string description) => new()
	{
		ReferenceNo = NewReferenceNo("TOP"),
		ReceiverWallet = receiver,
		Type = TransactionType.TopUp,
		Amount = amount,
		Status = TransactionStatus.Pending,
		Description = description,
		CreatedAtUtc = DateTime.UtcNow
	};

	public static Transaction CreateTransfer(Wallet sender, Wallet receiver, decimal amount, string description)
	{
		DateTime now = DateTime.UtcNow;
		return new()
		{
			ReferenceNo = NewReferenceNo("TRF"),
			SenderWallet = sender,
			ReceiverWallet = receiver,
			Type = TransactionType.Transfer,
			Amount = amount,
			Status = TransactionStatus.Success,
			Description = description,
			CreatedAtUtc = now,
			CompletedAtUtc = now
		};
	}

	public long Id { get; private set; }
	public string ReferenceNo { get; private set; } = "";
	public long? SenderWalletId { get; private set; }
	public long ReceiverWalletId { get; private set; }
	public string Type { get; private set; } = "";
	public decimal Amount { get; private set; }
	public string Status { get; private set; } = TransactionStatus.Pending;
	public string? Description { get; private set; }
	public DateTime CreatedAtUtc { get; private set; }
	public DateTime? CompletedAtUtc { get; private set; }

	public Wallet? SenderWallet { get; private set; }
	public Wallet ReceiverWallet { get; private set; } = null!;

	public void MarkSuccess()
	{
		Status = TransactionStatus.Success;
		CompletedAtUtc = DateTime.UtcNow;
	}

	private static string NewReferenceNo(string prefix) =>
		$"{prefix}-{DateTime.UtcNow:yyyyMMddHHmmss}-{Guid.NewGuid().ToString("N")[..6].ToUpperInvariant()}";
}