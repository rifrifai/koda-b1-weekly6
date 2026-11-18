using KodaEWallet.Data;
using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.DTOs.Transaction;
using KodaEWallet.DTOs.WalletMutation;
using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaEWallet.Repositories;

public class TransactionRepository(AppDbContext db) : ITransactionRepository
{
	public void AddMutation(WalletMutation mutation) => db.WalletMutations.Add(mutation);

	public void AddTopUp(TopUpRequest topUp) => db.TopUpRequests.Add(topUp);

	public void AddTransaction(Transaction transaction) => db.Transactions.Add(transaction);

	public Task<TopUpRequest?> GetTopUpAsync(long id, CancellationToken ct) =>
		db.TopUpRequests
			.Include(t => t.Transaction)
			.Include(t => t.Wallet)
			.FirstOrDefaultAsync(t => t.Id == id, ct);

	public async Task<PagedResponse<MutationResponse>> SearchMutationsAsync(long walletId, HistoryQuery query, CancellationToken ct)
	{
		IQueryable<WalletMutation> q = db.WalletMutations.AsNoTracking()
			.Where(m => m.WalletId == walletId);

		if (!string.IsNullOrWhiteSpace(query.Search))
		{
			string search = query.Search.Trim();
			q = q.Where(m => m.Transaction.ReferenceNo.Contains(search));
		}
		if (query.Type != null)
		{
			string type = query.Type;
			q = q.Where(m => m.Transaction.Type == type);
		}
		if (query.Status != null)
		{
			string status = query.Status;
			q = q.Where(m => m.Transaction.Status == status);
		}
		if (query.Direction != null)
		{
			string direction = query.Direction;
			q = q.Where(m => m.Direction == direction);
		}
		if (query.DateFrom != null)
		{
			DateTime from = query.DateFrom.Value.Date;
			q = q.Where(m => m.CreatedAtUtc >= from);
		}
		if (query.DateTo != null)
		{
			DateTime toExclusive = query.DateTo.Value.Date.AddDays(1);
			q = q.Where(m => m.CreatedAtUtc < toExclusive);
		}

		bool desc = query.SortDir != "asc";
		q = (query.SortBy, desc) switch
		{
			("amount", true) => q.OrderByDescending(m => m.Amount).ThenByDescending(m => m.Id),
			("amount", false) => q.OrderBy(m => m.Amount).ThenBy(m => m.Id),
			(_, true) => q.OrderByDescending(m => m.CreatedAtUtc).ThenByDescending(m => m.Id),
			_ => q.OrderBy(m => m.CreatedAtUtc).ThenBy(m => m.Id)
		};

		int total = await q.CountAsync(ct);

		List<MutationResponse> items = await q
			.Skip((query.Page - 1) * query.PageSize)
			.Take(query.PageSize)
			.Select(m => new MutationResponse
			{
				Id = m.Id,
				ReferenceNo = m.Transaction.ReferenceNo,
				TransactionType = m.Transaction.Type,
				Direction = m.Direction,
				Amount = m.Amount,
				BalanceBefore = m.BalanceBefore,
				BalanceAfter = m.BalanceAfter,
				CreatedAtUtc = m.CreatedAtUtc
			})
			.ToListAsync(ct);

		return new PagedResponse<MutationResponse>
		{
			Page = query.Page,
			PageSize = query.PageSize,
			TotalCount = total,
			Items = items
		};
	}

	public async Task<PagedResponse<TransactionResponse>> SearchTransactionsAsync(long walletId, HistoryQuery query, CancellationToken ct)
	{
		IQueryable<Transaction> q = db.Transactions.AsNoTracking()
			.Where(t => t.SenderWalletId == walletId || t.ReceiverWalletId == walletId);

		if (!string.IsNullOrWhiteSpace(query.Search))
		{
			string search = query.Search.Trim();
			q = q.Where(t => t.ReferenceNo.Contains(search));
		}
		if (query.Type != null)
		{
			string type = query.Type;
			q = q.Where(t => t.Type == type);
		}
		if (query.Status != null)
		{
			string status = query.Status;
			q = q.Where(t => t.Status == status);
		}
		if (query.Direction == MutationDirection.In)
		{
			q = q.Where(t => t.ReceiverWalletId == walletId);
		}
		else if (query.Direction == MutationDirection.Out)
		{
			q = q.Where(t => t.SenderWalletId == walletId);
		}
		if (query.DateFrom != null)
		{
			DateTime from = query.DateFrom.Value.Date;
			q = q.Where(t => t.CreatedAtUtc >= from);
		}
		if (query.DateTo != null)
		{
			DateTime toExclusive = query.DateTo.Value.Date.AddDays(1);
			q = q.Where(t => t.CreatedAtUtc < toExclusive);
		}

		bool desc = query.SortDir != "asc";
		q = (query.SortBy, desc) switch
		{
			("amount", true) => q.OrderByDescending(t => t.Amount).ThenByDescending(t => t.Id),
			("amount", false) => q.OrderBy(t => t.Amount).ThenBy(t => t.Id),
			(_, true) => q.OrderByDescending(t => t.CreatedAtUtc).ThenByDescending(t => t.Id),
			_ => q.OrderBy(t => t.CreatedAtUtc).ThenBy(t => t.Id)
		};

		int total = await q.CountAsync(ct);

		List<TransactionResponse> items = await q
			.Skip((query.Page - 1) * query.PageSize)
			.Take(query.PageSize)
			.Select(t => new TransactionResponse
			{
				Id = t.Id,
				ReferenceNo = t.ReferenceNo,
				Type = t.Type,
				Direction = t.ReceiverWalletId == walletId ? MutationDirection.In : MutationDirection.Out,
				Amount = t.Amount,
				Status = t.Status,
				Description = t.Description,
				SenderWalletNumber = t.SenderWallet == null ? null : t.SenderWallet.WalletNumber,
				ReceiverWalletNumber = t.ReceiverWallet.WalletNumber,
				CreatedAtUtc = t.CreatedAtUtc,
				CompletedAtUtc = t.CompletedAtUtc
			})
			.ToListAsync(ct);

		return new PagedResponse<TransactionResponse>
		{
			Page = query.Page,
			PageSize = query.PageSize,
			TotalCount = total,
			Items = items
		};
	}
}