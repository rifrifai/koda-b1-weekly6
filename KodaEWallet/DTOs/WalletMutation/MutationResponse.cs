namespace KodaEWallet.DTOs.WalletMutation;

public sealed class MutationResponse
{
	public long Id { get; init; }
	public string ReferenceNo { get; init; } = "";
	public string TransactionType { get; init; } = "";
	public string Direction { get; init; } = "";
	public decimal Amount { get; init; }
	public decimal BalanceBefore { get; init; }
	public decimal BalanceAfter { get; init; }
	public DateTime CreatedAtUtc { get; init; }
}