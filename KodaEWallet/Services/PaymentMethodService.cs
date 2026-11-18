using KodaEWallet.DTOs.Payment;
using KodaEWallet.Repositories;

namespace KodaEWallet.Services;

public class PaymentMethodService(IPaymentMethodRepository repository) : IPaymentMethodService
{
	public Task<List<PaymentMethodResponse>> GetActiveAsync(CancellationToken ct) => repository.GetActiveAsync(ct);
}