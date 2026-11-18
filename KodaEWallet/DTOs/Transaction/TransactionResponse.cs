namespace KodaEWallet.DTOs.Transaction;

public class TransactionResponse
{
	public long Id { get; init; }
	public string ReferenceNo { get; init; } = "";
	public string Type { get; init; } = "";
	public string Direction { get; init; } = "";
	public decimal Amount { get; init; }
	public string Status { get; init; } = "";
	public string? Description { get; init; }
	public string? SenderWalletNumber { get; init; } 
	public string ReceiverWalletNumber { get; init; } = "";
	public DateTime CreatedAtUtc { get; init; }
	public DateTime? CompletedAtUtc { get; init; }
}