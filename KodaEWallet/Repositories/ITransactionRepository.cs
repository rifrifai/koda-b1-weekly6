using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.DTOs.Transaction;
using KodaEWallet.DTOs.WalletMutation;
using KodaEWallet.Models;

namespace KodaEWallet.Repositories;

public interface ITransactionRepository
{
    void AddTransaction(Transaction transaction);
    void AddMutation(WalletMutation mutation);
    void AddTopUp(TopUpRequest topUp);
    Task<TopUpRequest?> GetTopUpAsync(long id, CancellationToken ct);

    Task<PagedResponse<TransactionResponse>> SearchTransactionsAsync(long walletId, HistoryQuery query, CancellationToken ct);
    Task<PagedResponse<MutationResponse>> SearchMutationsAsync(long walletId, HistoryQuery query, CancellationToken ct);
}