using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.DTOs.Payment;
using KodaEWallet.DTOs.TopUp;
using KodaEWallet.DTOs.Transaction;
using KodaEWallet.DTOs.Wallet;
using KodaEWallet.DTOs.WalletMutation;
using KodaEWallet.Exceptions;
using KodaEWallet.Models;
using KodaEWallet.Repositories;

namespace KodaEWallet.Services;

public class WalletService(IWalletRepository wallets, ITransactionRepository transactions, IPaymentMethodRepository paymentMethods, IUserRepository users, IAuditRepository audits, IUnitOfWork uow) : IWalletService
{
	public async Task<WalletResponse> GetWalletAsync(long userId, CancellationToken ct)
	{
		return await wallets.GetInfoAsync(userId, ct)
			?? throw new NotFoundException($"Wallet untuk user dengan id {userId} tidak ditemukan");
	}

	public async Task<TopUpResponse> RequestTopUpAsync(long userId, CreateTopUpRequest request, CancellationToken ct)
	{
		ValidateAmount(request.Amount);

		Wallet wallet = await FindWalletAsync(userId, ct);
		EnsureActive(wallet, "tujuan");

		string code = request.PaymentMethodCode.Trim().ToUpperInvariant();
		PaymentMethod method = await paymentMethods.GetActiveByCodeAsync(code, ct)
			?? throw new NotFoundException($"Metode pembayaran {code} tidak ditemukan atau tidak aktif");

		var transaction = Transaction.CreateTopUp(wallet, request.Amount, DefaultIfEmpty(request.Description, "Top up saldo"));
		var topUp = TopUpRequest.Create(transaction, wallet, method);

		transactions.AddTransaction(transaction);
		transactions.AddTopUp(topUp);
		audits.Add(AuditLog.Create(AuditAction.TopUpRequested, transaction.ReferenceNo,
			AuditOutcome.Success, "Permintaan top up dibuat", userId, transaction));

		await uow.SaveChangesAsync(ct);

		return new TopUpResponse
		{
			TopUpRequestId = topUp.Id,
			ReferenceNo = transaction.ReferenceNo,
			WalletNumber = wallet.WalletNumber,
			PaymentMethod = method.Name,
			Amount = topUp.Amount,
			Status = topUp.Status,
			ProviderReference = topUp.ProviderReference,
			ExpiresAtUtc = topUp.ExpiresAtUtc
		};
	}

	public async Task<TopUpConfirmResponse> ConfirmTopUpAsync(long topUpRequestId, int delayMs, CancellationToken ct)
	{
		await using var tx = await uow.BeginTransactionAsync(ct);

		TopUpRequest topUp = await transactions.GetTopUpAsync(topUpRequestId, ct)
			?? throw new NotFoundException($"Top up dengan id {topUpRequestId} tidak ditemukan");

		if (topUp.Status != TopUpStatus.Pending)
		{
			throw new ConflictException($"Top up sudah diproses (status {topUp.Status})");
		}
		if (topUp.ExpiresAtUtc < DateTime.UtcNow)
		{
			throw new BadRequestException("Batas waktu pembayaran top up sudah lewat");
		}

		Wallet wallet = topUp.Wallet;
		Transaction transaction = topUp.Transaction;
		EnsureActive(wallet, "tujuan");

		if (delayMs > 0)
		{
			await Task.Delay(delayMs, ct);
		}

		decimal balanceBefore = wallet.Balance;
		wallet.Credit(topUp.Amount);
		topUp.MarkPaid();
		transaction.MarkSuccess();
		transactions.AddMutation(WalletMutation.Create(
			transaction, wallet, MutationDirection.In, topUp.Amount, balanceBefore, wallet.Balance));

		audits.Add(AuditLog.Create(AuditAction.TopUpPaid, transaction.ReferenceNo,
			AuditOutcome.Success, "Pembayaran top up dikonfirmasi", wallet.UserId, transaction));

		await uow.SaveChangesAsync(ct);
		await tx.CommitAsync(ct);

		return new TopUpConfirmResponse
		{
			ReferenceNo = transaction.ReferenceNo,
			Status = topUp.Status,
			Amount = topUp.Amount,
			BalanceBefore = balanceBefore,
			BalanceAfter = wallet.Balance,
			PaidAtUtc = topUp.PaidAtUtc
		};
	}

	public async Task<TransferResponse> TransferAsync(TransferRequest request, CancellationToken ct)
	{
		try
		{
			return await ProcessTransferAsync(request, ct);
		}
		catch (AppException ex)
		{
			await LogRejectedTransferAsync(request.SenderUserId, ex.Message, ct);
			throw;
		}
	}

	private async Task<TransferResponse> ProcessTransferAsync(TransferRequest request, CancellationToken ct)
	{
		ValidateAmount(request.Amount);

		await using var tx = await uow.BeginTransactionAsync(ct);

		Wallet sender = await FindWalletAsync(request.SenderUserId, ct);

		string receiverNumber = request.ReceiverWalletNumber.Trim().ToUpperInvariant();
		Wallet receiver = await wallets.GetByNumberAsync(receiverNumber, ct)
			?? throw new NotFoundException($"Wallet dengan nomor {receiverNumber} tidak ditemukan");

		if (receiver.Id == sender.Id)
		{
			throw new BadRequestException("Tidak bisa transfer ke wallet sendiri");
		}
		EnsureActive(sender, "pengirim");
		EnsureActive(receiver, "penerima");
		if (sender.Balance < request.Amount)
		{
			throw new InsufficientBalanceException();
		}

		decimal senderBefore = sender.Balance;
		decimal receiverBefore = receiver.Balance;

		sender.Debit(request.Amount);
		receiver.Credit(request.Amount);

		var transaction = Transaction.CreateTransfer(sender, receiver, request.Amount,
			DefaultIfEmpty(request.Description, "Transfer saldo"));

		transactions.AddTransaction(transaction);
		transactions.AddMutation(WalletMutation.Create(
			transaction, sender, MutationDirection.Out, request.Amount, senderBefore, sender.Balance));
		transactions.AddMutation(WalletMutation.Create(
			transaction, receiver, MutationDirection.In, request.Amount, receiverBefore, receiver.Balance));
		audits.Add(AuditLog.Create(AuditAction.TransferCreated, transaction.ReferenceNo,
			AuditOutcome.Success, "Transfer berhasil", sender.UserId, transaction));

		await uow.SaveChangesAsync(ct);
		await tx.CommitAsync(ct);

		return new TransferResponse
		{
			ReferenceNo = transaction.ReferenceNo,
			Status = transaction.Status,
			SenderName = sender.User.FullName,
			SenderWalletNumber = sender.WalletNumber,
			ReceiverName = receiver.User.FullName,
			ReceiverWalletNumber = receiver.WalletNumber,
			Amount = request.Amount,
			SenderBalanceBefore = senderBefore,
			SenderBalanceAfter = sender.Balance
		};
	}

	private async Task LogRejectedTransferAsync(long senderUserId, string reason, CancellationToken ct)
	{
		uow.ClearTracking();

		long? actor = await users.ExistsAsync(senderUserId, ct) ? senderUserId : null;

		audits.Add(AuditLog.Create(AuditAction.TransferRejected, null,
			AuditOutcome.Rejected, $"Transfer ditolak: {reason}", actor, null));
		await uow.SaveChangesAsync(ct);
	}

	public async Task<PagedResponse<TransactionResponse>> GetTransactionsAsync(long userId, HistoryQuery query, CancellationToken ct)
	{
		long walletId = await FindWalletIdAsync(userId, ct);
		return await transactions.SearchTransactionsAsync(walletId, query, ct);
	}
	public async Task<PagedResponse<MutationResponse>> GetMutationsAsync(long userId, HistoryQuery query, CancellationToken ct)
	{
		long walletId = await FindWalletIdAsync(userId, ct);
		return await transactions.SearchMutationsAsync(walletId, query, ct);
	}

	private async Task<Wallet> FindWalletAsync(long userId, CancellationToken ct) => await wallets.GetByUserIdAsync(userId, ct)
		?? throw new NotFoundException($"Wallet untuk user dengan id {userId} tidak ditemukan");

	private async Task<long> FindWalletIdAsync(long userId, CancellationToken ct) => await wallets.GetIdByUserIdAsync(userId, ct)
		?? throw new NotFoundException($"Wallet untuk user dengan id {userId} tidak ditemukan");

	private static void EnsureActive(Wallet wallet, string role)
	{
		if (!wallet.IsActive)
		{
			throw new BadRequestException($"Wallet {role} tidak aktif (status {wallet.Status})");
		}
	}

	private static void ValidateAmount(decimal amount)
	{
		if (amount <= 0)
		{
			throw new BadRequestException("Nominal harus lebih dari 0");
		}
		if (decimal.Round(amount, 2) != amount)
		{
			throw new BadRequestException("Nominal maksimal 2 angka di belakang koma");
		}
	}

	private static string DefaultIfEmpty(string? text, string fallback) => string.IsNullOrWhiteSpace(text) ? fallback : text.Trim();
}