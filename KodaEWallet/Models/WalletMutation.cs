namespace KodaEWallet.Models;

public class WalletMutation
{
	private WalletMutation() { }

	public static WalletMutation Create(Transaction transaction, Wallet wallet, string direction,
		decimal amount, decimal balanceBefore, decimal balanceAfter) => new()
	{
		Transaction = transaction,
		Wallet = wallet,
		Direction = direction,
		Amount = amount,
		BalanceBefore = balanceBefore,
		BalanceAfter = balanceAfter,
		CreatedAtUtc = DateTime.UtcNow
	};

	public long Id { get; private set; }
	public long TransactionId { get; private set; }
	public long WalletId { get; private set; }
	public string Direction { get; private set; } = "";
	public decimal Amount { get; private set; }
	public decimal BalanceBefore { get; private set; }
	public decimal BalanceAfter { get; private set; }
	public DateTime CreatedAtUtc { get; private set; }

	public Transaction Transaction { get; private set; } = null!;
	public Wallet Wallet { get; private set; } = null!;
}