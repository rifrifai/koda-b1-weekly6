namespace KodaEWallet.DTOs.Payment;

public class TransferResponse
{
	public string ReferenceNo { get; init; } = "";
	public string Status { get; init; } = "";
	public string SenderName { get; init; } = "";
	public string SenderWalletNumber { get; init; } = "";
	public string ReceiverName { get; init; } = "";
	public string ReceiverWalletNumber { get; init; } = "";
	public decimal Amount { get; init; }
	public decimal SenderBalanceBefore { get; init; }
	public decimal SenderBalanceAfter { get; init; }
}