namespace KodaEWallet.Models;

public class TopUpRequest
{
	private TopUpRequest() { }

	public static TopUpRequest Create(Transaction transaction, Wallet wallet, PaymentMethod paymentMethod)
	{
		DateTime now = DateTime.UtcNow;
		return new()
		{
			Transaction = transaction,
			Wallet = wallet,
			PaymentMethod = paymentMethod,
			Amount = transaction.Amount,
			Status = TopUpStatus.Pending,
			ProviderReference = "SIM-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant(),
			CreatedAtUtc = now,
			ExpiresAtUtc = now.AddHours(24)
		};
	}

	public long Id { get; private set; }
	public long TransactionId { get; private set; }
	public long WalletId { get; private set; }
	public int PaymentMethodId { get; private set; }
	public decimal Amount { get; private set; }
	public string Status { get; private set; } = TopUpStatus.Pending;
	public string? ProviderReference { get; private set; }
	public DateTime CreatedAtUtc { get; private set; }
	public DateTime? ExpiresAtUtc { get; private set; }
	public DateTime? PaidAtUtc { get; private set; }

	public Transaction Transaction { get; private set; } = null!;
	public Wallet Wallet { get; private set; } = null!;
	public PaymentMethod PaymentMethod { get; private set; } = null!;

	public void MarkPaid()
	{
		Status = TopUpStatus.Paid;
		PaidAtUtc = DateTime.UtcNow;
	}
}