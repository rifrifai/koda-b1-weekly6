namespace KodaEWallet.DTOs.TopUp;

public class TopUpConfirmResponse
{
	public string ReferenceNo { get; init; } = "";
	public string Status { get; init; } = "";
	public decimal Amount { get; init; }
	public decimal BalanceBefore { get; init; }
	public decimal BalanceAfter { get; init; }
	public DateTime? PaidAtUtc { get; init; }
}