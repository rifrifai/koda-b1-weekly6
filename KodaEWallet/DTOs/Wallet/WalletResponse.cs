namespace KodaEWallet.DTOs.Wallet;

public class WalletResponse
{
    public long UserId { get; init; }
    public string OwnerName { get; init; } = "";
    public string WalletNumber { get; init; } = "";
    public decimal Balance { get; init; }
    public string Status { get; init; } = "";
}
