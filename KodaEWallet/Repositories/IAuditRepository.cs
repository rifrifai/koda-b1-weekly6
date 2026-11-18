using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.Models;

namespace KodaEWallet.Repositories;

public interface IAuditRepository
{
	void Add(AuditLog log);
	Task<PagedResponse<AuditLogResponse>> GetPageAsync(int page, int pageSize, CancellationToken ct);
}