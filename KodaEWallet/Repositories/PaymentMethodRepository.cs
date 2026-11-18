using KodaEWallet.Data;
using KodaEWallet.DTOs.Payment;
using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaEWallet.Repositories;

public class PaymentMethodRepository(AppDbContext db) : IPaymentMethodRepository
{
	public Task<List<PaymentMethodResponse>> GetActiveAsync(CancellationToken ct) => db.PaymentMethods.AsNoTracking().Where(p => p.IsActive).OrderBy(p => p.Id).Select(p => new PaymentMethodResponse
	{
		Id = p.Id,
		Code = p.Code,
		Name = p.Name,
		ProviderName = p.ProviderName
	}).ToListAsync(ct);

	public Task<PaymentMethod?> GetActiveByCodeAsync(string code, CancellationToken ct) => db.PaymentMethods.FirstOrDefaultAsync(p => p.Code == code && p.IsActive, ct);
}