using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaEWallet.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
	public DbSet<User> Users => Set<User>();
	public DbSet<Wallet> Wallets => Set<Wallet>();
	public DbSet<Transaction> Transactions => Set<Transaction>();
	public DbSet<WalletMutation> WalletMutations => Set<WalletMutation>();
	public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
	public DbSet<TopUpRequest> TopUpRequests => Set<TopUpRequest>();
	public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
	}
}