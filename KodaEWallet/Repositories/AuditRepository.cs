using KodaEWallet.Data;
using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaEWallet.Repositories;

public class AuditRepository(AppDbContext db) : IAuditRepository
{
	public void Add(AuditLog log) => db.AuditLogs.Add(log);

	public async Task<PagedResponse<AuditLogResponse>> GetPageAsync(int page, int pageSize, CancellationToken ct)
	{
		IQueryable<AuditLog> query = db.AuditLogs.AsNoTracking();

		int total = await query.CountAsync(ct);

		List<AuditLogResponse> items = await query
			.OrderByDescending(a => a.CreatedAtUtc).ThenByDescending(a => a.Id)
			.Skip((page - 1) * pageSize)
			.Take(pageSize)
			.Select(a => new AuditLogResponse
			{
				Id = a.Id,
				ActorUserId = a.ActorUserId,
				TransactionId = a.TransactionId,
				Action = a.Action,
				EntityType = a.EntityType,
				EntityId = a.EntityId,
				Outcome = a.Outcome,
				Description = a.Description,
				CorrelationId = a.CorrelationId,
				CreatedAtUtc = a.CreatedAtUtc
			})
			.ToListAsync(ct);

		return new PagedResponse<AuditLogResponse>
		{
			Page = page,
			PageSize = pageSize,
			TotalCount = total,
			Items = items
		};
	}
}
