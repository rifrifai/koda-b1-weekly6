using KodaEWallet.DTOs.Wallet;
using KodaEWallet.Models;

namespace KodaEWallet.Repositories;

public interface IWalletRepository
{
    Task<Wallet?> GetByUserIdAsync(long userId, CancellationToken ct);
    Task<Wallet?> GetByNumberAsync(string walletNumber, CancellationToken ct);
    Task<long?> GetIdByUserIdAsync(long userId, CancellationToken ct);
    Task<WalletResponse?> GetInfoAsync(long userId, CancellationToken ct);
    void Add(Wallet wallet);
}