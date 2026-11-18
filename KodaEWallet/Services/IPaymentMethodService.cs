using KodaEWallet.DTOs.Payment;

namespace KodaEWallet.Services;

public interface IPaymentMethodService
{
	Task<List<PaymentMethodResponse>> GetActiveAsync(CancellationToken ct);
}