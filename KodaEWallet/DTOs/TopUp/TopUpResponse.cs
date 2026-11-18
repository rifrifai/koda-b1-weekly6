namespace KodaEWallet.DTOs.TopUp;

public sealed class TopUpResponse
{
	public long TopUpRequestId { get; init; }
	public string ReferenceNo { get; init; } = "";
	public string WalletNumber { get; init; } = "";
	public string PaymentMethod { get; init; } = "";
	public decimal Amount { get; init; }
	public string Status { get; init; } = "";
	public string? ProviderReference { get; init; }
	public DateTime? ExpiresAtUtc { get; init; }
}