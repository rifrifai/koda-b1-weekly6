using KodaEWallet.DTOs.Payment;
using KodaEWallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaEWallet.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TransfersController(IWalletService service) : ControllerBase
{
	[HttpPost]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
	public async Task<ActionResult<TransferResponse>> Transfer(TransferRequest request, CancellationToken ct)
	{
		return Ok(await service.TransferAsync(request, ct));
	}
}
