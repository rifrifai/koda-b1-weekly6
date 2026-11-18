using Microsoft.EntityFrameworkCore.Storage;

namespace KodaEWallet.Repositories;

public interface IUnitOfWork
{
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    void ClearTracking();
}