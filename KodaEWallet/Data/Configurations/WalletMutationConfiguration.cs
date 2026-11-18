using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaEWallet.Data.Configurations;

public class WalletMutationConfiguration : IEntityTypeConfiguration<WalletMutation>
{
	public void Configure(EntityTypeBuilder<WalletMutation> builder)
	{
		builder.ToTable("WalletMutations", t =>
		{
			t.HasCheckConstraint("CK_WalletMutations_Amount", "[Amount] > 0");
			t.HasCheckConstraint("CK_WalletMutations_Direction", "[Direction] IN ('IN','OUT')");
			t.HasCheckConstraint("CK_WalletMutations_Balance",
				"([Direction] = 'IN' AND [BalanceAfter] = [BalanceBefore] + [Amount]) OR " +
				"([Direction] = 'OUT' AND [BalanceAfter] = [BalanceBefore] - [Amount])");
		});

		builder.Property(x => x.Direction).HasMaxLength(3).IsUnicode(false);
		builder.Property(x => x.Amount).HasPrecision(18, 2);
		builder.Property(x => x.BalanceBefore).HasPrecision(18, 2);
		builder.Property(x => x.BalanceAfter).HasPrecision(18, 2);

		builder.HasIndex(x => new { x.TransactionId, x.WalletId }).IsUnique();
		builder.HasIndex(x => new { x.WalletId, x.CreatedAtUtc, x.Id }).IsDescending(false, true, true);

		builder.HasOne(x => x.Transaction).WithMany().HasForeignKey(x => x.TransactionId).OnDelete(DeleteBehavior.Restrict);
		builder.HasOne(x => x.Wallet).WithMany().HasForeignKey(x => x.WalletId).OnDelete(DeleteBehavior.Restrict);
	}
}