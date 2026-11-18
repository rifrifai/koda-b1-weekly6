using KodaEWallet.Exceptions;

namespace KodaEWallet.Models;

public sealed class Wallet
{
	private Wallet() { }

	public static Wallet Create(long userId, string walletNumber) => new()
	{
		UserId = userId,
		WalletNumber = walletNumber,
		Balance = 0,
		Status = WalletStatus.Active,
		CreatedAtUtc = DateTime.UtcNow
	};

	public long Id { get; private set; }
	public long UserId { get; private set; }
	public string WalletNumber { get; private set; } = "";
	public decimal Balance { get; private set; }
	public string Status { get; private set; } = WalletStatus.Active;
	public byte[] RowVersion { get; private set; } = [];
	public DateTime CreatedAtUtc { get; private set; }
	public DateTime? UpdatedAtUtc { get; private set; }

	public User User { get; private set; } = null!; 

	public bool IsActive => Status == WalletStatus.Active;

	public void Credit(decimal amount)
	{
		Balance += amount;
		UpdatedAtUtc = DateTime.UtcNow;
	}

	public void Debit(decimal amount)
	{
		if (amount > Balance)
		{
			throw new InsufficientBalanceException();
		}
		Balance -= amount;
		UpdatedAtUtc = DateTime.UtcNow;
	}
}