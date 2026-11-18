using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.Repositories;

namespace KodaEWallet.Services;

public class AuditService(IAuditRepository repository) : IAuditService
{
	public Task<PagedResponse<AuditLogResponse>> GetPageAsync(int page, int pageSize, CancellationToken ct) => repository.GetPageAsync(page, pageSize, ct);
}
