using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;

namespace KodaEWallet.Services;

public interface IAuditService
{
	Task<PagedResponse<AuditLogResponse>> GetPageAsync(int page, int pageSize, CancellationToken ct);
}