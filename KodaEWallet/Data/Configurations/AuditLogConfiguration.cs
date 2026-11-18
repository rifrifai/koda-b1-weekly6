using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaEWallet.Data.Configurations;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
	public void Configure(EntityTypeBuilder<AuditLog> builder)
	{
		builder.ToTable("AuditLogs", t =>
			t.HasCheckConstraint("CK_AuditLogs_Outcome", "[Outcome] IN ('SUCCESS','REJECTED','FAILED')"));

		builder.Property(x => x.Action).HasMaxLength(60);
		builder.Property(x => x.EntityType).HasMaxLength(60);
		builder.Property(x => x.EntityId).HasMaxLength(60);
		builder.Property(x => x.Outcome).HasMaxLength(20).IsUnicode(false);
		builder.Property(x => x.Description).HasMaxLength(500);
		builder.Property(x => x.CorrelationId).HasMaxLength(100);

		builder.HasIndex(x => new { x.ActorUserId, x.CreatedAtUtc }).IsDescending(false, true);

		builder.HasOne<User>()
			.WithMany()
			.HasForeignKey(x => x.ActorUserId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Transaction)
			.WithMany()
			.HasForeignKey(x => x.TransactionId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}