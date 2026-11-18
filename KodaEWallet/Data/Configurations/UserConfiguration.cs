using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KodaEWallet.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.ToTable("Users", t =>
			t.HasCheckConstraint("CK_Users_Status", "[Status] IN ('ACTIVE','SUSPENDED')"));

		builder.Property(x => x.Username).HasMaxLength(50);
		builder.Property(x => x.FullName).HasMaxLength(100);
		builder.Property(x => x.Email).HasMaxLength(150);
		builder.Property(x => x.Phone).HasMaxLength(20);
		builder.Property(x => x.PasswordHash).HasMaxLength(500);
		builder.Property(x => x.Status).HasMaxLength(20).IsUnicode(false);

		builder.HasIndex(x => x.Username).IsUnique();
		builder.HasIndex(x => x.Email).IsUnique();
		builder.HasIndex(x => x.Phone).IsUnique();
	}
}