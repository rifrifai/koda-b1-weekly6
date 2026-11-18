using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaEWallet.Data.Configurations;

public class TopUpRequestConfiguration : IEntityTypeConfiguration<TopUpRequest>
{
	public void Configure(EntityTypeBuilder<TopUpRequest> builder)
	{
		builder.ToTable("TopUpRequests", t =>
		{
			t.HasCheckConstraint("CK_TopUpRequests_Amount", "[Amount] > 0");
			t.HasCheckConstraint("CK_TopUpRequests_Status",
				"[Status] IN ('PENDING','PAID','FAILED','EXPIRED')");
		});

		builder.Property(x => x.Amount).HasPrecision(18, 2);
		builder.Property(x => x.Status).HasMaxLength(20).IsUnicode(false);
		builder.Property(x => x.ProviderReference).HasMaxLength(100);

		builder.HasOne(x => x.Transaction)
			.WithOne()
			.HasForeignKey<TopUpRequest>(x => x.TransactionId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.Wallet)
			.WithMany()
			.HasForeignKey(x => x.WalletId)
			.OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(x => x.PaymentMethod)
			.WithMany()
			.HasForeignKey(x => x.PaymentMethodId)
			.OnDelete(DeleteBehavior.Restrict);
	}
}