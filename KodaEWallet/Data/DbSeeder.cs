using KodaEWallet.DTOs.Payment;
using KodaEWallet.DTOs.TopUp;
using KodaEWallet.DTOs.User;
using KodaEWallet.DTOs.Wallet;
using KodaEWallet.Models;
using KodaEWallet.Services;
using Microsoft.EntityFrameworkCore;

namespace KodaEWallet.Data;

public static class DbSeeder
{
	public static async Task SeedAsync(IServiceProvider services, CancellationToken ct = default)
	{
		using IServiceScope scope = services.CreateScope();
		var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
		var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
		var walletService = scope.ServiceProvider.GetRequiredService<IWalletService>();

		if (await db.Users.AnyAsync(ct))
		{
			return;
		}

		db.PaymentMethods.AddRange(
			PaymentMethod.Create("BANK_TRANSFER", "Bank Transfer", "Simulasi Bank"),
			PaymentMethod.Create("QRIS", "QRIS", "Simulasi QRIS"));
		await db.SaveChangesAsync(ct);

		string[] names = ["Andi", "Budi", "Citra", "Rizal", "Eko", "Fitri", "Afif", "Alvin", "Indah", "Joko"];
		var userIds = new List<long>();
		for (int i = 0; i < names.Length; i++)
		{
			UserResponse user = await userService.CreateAsync(new CreateUserRequest
			{
				Username = names[i].ToLowerInvariant(),
				FullName = names[i],
				Email = $"{names[i].ToLowerInvariant()}@mail.com",
				Phone = $"08120000{i + 1:D4}"
			}, ct);
			userIds.Add(user.Id);
		}

		await TopUpPaidAsync(userIds[0], 500_000, "BANK_TRANSFER");
		await TopUpPaidAsync(userIds[1], 350_000, "QRIS");
		await TopUpPaidAsync(userIds[2], 600_000, "BANK_TRANSFER");
		await TopUpPaidAsync(userIds[3], 250_000, "QRIS");

		await TransferAsync(userIds[2], userIds[0], 300_000, "Traktir makan warung gatcha");
		await TransferAsync(userIds[1], userIds[2], 50_000, "Bayar hutang kas");
		await TransferAsync(userIds[3], userIds[4], 100_000, "Patungan hadiah");

		await walletService.RequestTopUpAsync(userIds[5], new CreateTopUpRequest
		{
			Amount = 150_000,
			PaymentMethodCode = "QRIS",
			Description = "Top up menunggu pembayaran"
		}, ct);

		async Task TopUpPaidAsync(long userId, decimal amount, string methodCode)
		{
			TopUpResponse request = await walletService.RequestTopUpAsync(userId, new CreateTopUpRequest
			{
				Amount = amount,
				PaymentMethodCode = methodCode,
				Description = "Top up awal"
			}, ct);
			await walletService.ConfirmTopUpAsync(request.TopUpRequestId, 0, ct);
		}

		async Task TransferAsync(long fromUserId, long toUserId, decimal amount, string description)
		{
			WalletResponse receiver = await walletService.GetWalletAsync(toUserId, ct);
			await walletService.TransferAsync(new TransferRequest
			{
				SenderUserId = fromUserId,
				ReceiverWalletNumber = receiver.WalletNumber,
				Amount = amount,
				Description = description
			}, ct);
		}
	}
}