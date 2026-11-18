using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaEWallet.Data.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
{
	public void Configure(EntityTypeBuilder<Transaction> builder)
	{
		builder.ToTable("Transactions", t =>
		{
			t.HasCheckConstraint("CK_Transactions_Amount", "[Amount] > 0");
			t.HasCheckConstraint("CK_Transactions_Type", "[Type] IN ('TOPUP','TRANSFER')");
			t.HasCheckConstraint("CK_Transactions_Status", "[Status] IN ('PENDING','SUCCESS','FAILED')");
			t.HasCheckConstraint("CK_Transactions_Shape",
				"([Type] = 'TOPUP' AND [SenderWalletId] IS NULL) OR " +
				"([Type] = 'TRANSFER' AND [SenderWalletId] IS NOT NULL AND [SenderWalletId] <> [ReceiverWalletId])");
		});

		builder.Property(x => x.ReferenceNo).HasMaxLength(50);
		builder.Property(x => x.Type).HasMaxLength(20).IsUnicode(false);
		builder.Property(x => x.Status).HasMaxLength(20).IsUnicode(false);
		builder.Property(x => x.Amount).HasPrecision(18, 2);
		builder.Property(x => x.Description).HasMaxLength(255);

		builder.HasIndex(x => x.ReferenceNo).IsUnique();
		builder.HasIndex(x => new { x.SenderWalletId, x.CreatedAtUtc }).IsDescending(false, true);
		builder.HasIndex(x => new { x.ReceiverWalletId, x.CreatedAtUtc }).IsDescending(false, true);

		builder.HasOne(x => x.SenderWallet).WithMany().HasForeignKey(x => x.SenderWalletId).OnDelete(DeleteBehavior.Restrict);
		builder.HasOne(x => x.ReceiverWallet).WithMany().HasForeignKey(x => x.ReceiverWalletId).OnDelete(DeleteBehavior.Restrict);
	}
}