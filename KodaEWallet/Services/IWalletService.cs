using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.DTOs.Payment;
using KodaEWallet.DTOs.TopUp;
using KodaEWallet.DTOs.Transaction;
using KodaEWallet.DTOs.Wallet;
using KodaEWallet.DTOs.WalletMutation;

namespace KodaEWallet.Services;

public interface IWalletService
{
	Task<WalletResponse> GetWalletAsync(long userId, CancellationToken ct);
	Task<TopUpResponse> RequestTopUpAsync(long userId, CreateTopUpRequest request, CancellationToken ct);
	Task<TopUpConfirmResponse> ConfirmTopUpAsync(long topUpRequestId, int delayMs, CancellationToken ct);
	Task<TransferResponse> TransferAsync(TransferRequest request, CancellationToken ct);
	Task<PagedResponse<TransactionResponse>> GetTransactionsAsync(long userId, HistoryQuery query, CancellationToken ct);
	Task<PagedResponse<MutationResponse>> GetMutationsAsync(long userId, HistoryQuery query, CancellationToken ct);
}