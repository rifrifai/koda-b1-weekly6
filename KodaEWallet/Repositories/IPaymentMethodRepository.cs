using KodaEWallet.DTOs.Payment;
using KodaEWallet.Models;

namespace KodaEWallet.Repositories;

public interface IPaymentMethodRepository
{
    Task<List<PaymentMethodResponse>> GetActiveAsync(CancellationToken ct);
    Task<PaymentMethod?> GetActiveByCodeAsync(string code, CancellationToken ct);
}