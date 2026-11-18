using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaEWallet.Data.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
	public void Configure(EntityTypeBuilder<PaymentMethod> builder)
	{
		builder.Property(x => x.Code).HasMaxLength(30);
		builder.Property(x => x.Name).HasMaxLength(100);
		builder.Property(x => x.ProviderName).HasMaxLength(100);

		builder.HasIndex(x => x.Code).IsUnique();
	}
}