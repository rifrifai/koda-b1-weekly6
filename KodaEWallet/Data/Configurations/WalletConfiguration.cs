using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaEWallet.Data.Configurations;

public class WalletConfiguration : IEntityTypeConfiguration<Wallet>
{
	public void Configure(EntityTypeBuilder<Wallet> builder)
	{
		builder.ToTable("Wallets", t =>
		{
			t.HasCheckConstraint("CK_Wallets_Balance", "[Balance] >= 0");
			t.HasCheckConstraint("CK_Wallets_Status", "[Status] IN ('ACTIVE','FROZEN','CLOSED')");
		});

		builder.Property(x => x.WalletNumber).HasMaxLength(30);
		builder.Property(x => x.Balance).HasPrecision(18, 2);
		builder.Property(x => x.Status).HasMaxLength(20).IsUnicode(false);
		builder.Property(x => x.RowVersion).IsRowVersion();

		builder.HasIndex(x => x.WalletNumber).IsUnique();

		builder.HasOne(x => x.User)
			.WithOne()
			.HasForeignKey<Wallet>(x => x.UserId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}