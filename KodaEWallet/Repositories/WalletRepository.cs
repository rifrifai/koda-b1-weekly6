using KodaEWallet.Data;
using KodaEWallet.DTOs.Wallet;
using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaEWallet.Repositories;

public class WalletRepository(AppDbContext db) : IWalletRepository
{
	public void Add(Wallet wallet) => db.Wallets.Add(wallet);

	public Task<Wallet?> GetByNumberAsync(string walletNumber, CancellationToken ct) =>
		db.Wallets.Include(w => w.User).FirstOrDefaultAsync(w => w.WalletNumber == walletNumber, ct);
	public Task<Wallet?> GetByUserIdAsync(long userId, CancellationToken ct) =>
		db.Wallets.Include(w => w.User).FirstOrDefaultAsync(w => w.UserId == userId, ct);

	public Task<long?> GetIdByUserIdAsync(long userId, CancellationToken ct) =>
		db.Wallets.Where(w => w.UserId == userId).Select(w => (long?)w.Id).FirstOrDefaultAsync(ct);

	public Task<WalletResponse?> GetInfoAsync(long userId, CancellationToken ct) =>
		db.Wallets.AsNoTracking()
			.Where(w => w.UserId == userId)
			.Select(w => new WalletResponse
			{
				UserId = w.UserId,
				OwnerName = w.User.FullName,
				WalletNumber = w.WalletNumber,
				Balance = w.Balance,
				Status = w.Status
			})
			.FirstOrDefaultAsync(ct);
}