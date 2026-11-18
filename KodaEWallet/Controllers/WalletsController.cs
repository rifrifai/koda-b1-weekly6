using KodaEWallet.DTOs.TopUp;
using KodaEWallet.DTOs.Wallet;
using KodaEWallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaEWallet.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class WalletsController(IWalletService service) : ControllerBase
{
    [HttpGet("{userId:long}")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<WalletResponse>> GetWallet(long userId, CancellationToken ct)
    {
        return Ok(await service.GetWalletAsync(userId, ct));
    }

    [HttpPost("{userId:long}/topups")]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TopUpResponse>> RequestTopUp(long userId, CreateTopUpRequest request, CancellationToken ct)
    {
        return Ok(await service.RequestTopUpAsync(userId, request, ct));
    }
}