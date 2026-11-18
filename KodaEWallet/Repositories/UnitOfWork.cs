using KodaEWallet.Data;
using Microsoft.EntityFrameworkCore.Storage;

namespace KodaEWallet.Repositories;

public class UnitOfWork(AppDbContext db) : IUnitOfWork
{
	public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken ct) => db.Database.BeginTransactionAsync(ct);
	public async Task SaveChangesAsync(CancellationToken ct) => await db.SaveChangesAsync(ct);
	public void ClearTracking() => db.ChangeTracker.Clear();
}