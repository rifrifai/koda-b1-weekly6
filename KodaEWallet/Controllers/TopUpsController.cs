using System.ComponentModel.DataAnnotations;
using KodaEWallet.DTOs.TopUp;
using KodaEWallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaEWallet.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class TopUpsController(IWalletService service, IWebHostEnvironment env) : ControllerBase
{
	[HttpPost("{id:long}/confirm")]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
	public async Task<ActionResult<TopUpConfirmResponse>> Confirm(long id, CancellationToken ct, [FromQuery, Range(0, 15000)] int delayMs = 0)
	{
		if (!env.IsDevelopment() && !env.IsEnvironment("Testing"))
		{
			return NotFound();
		}
		return Ok(await service.ConfirmTopUpAsync(id, delayMs, ct));
	}
}