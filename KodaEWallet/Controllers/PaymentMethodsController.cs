using KodaEWallet.DTOs.Payment;
using KodaEWallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaEWallet.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class PaymentMethodsController(IPaymentMethodService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PaymentMethodResponse>>> GetActive(CancellationToken ct)
    {
        return Ok(await service.GetActiveAsync(ct));
    }
}