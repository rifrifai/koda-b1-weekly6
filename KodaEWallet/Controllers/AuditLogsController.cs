using System.ComponentModel.DataAnnotations;
using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaEWallet.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuditLogsController(IAuditService service) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<PagedResponse<AuditLogResponse>>> GetPage(
		CancellationToken ct,
		[FromQuery, Range(1, int.MaxValue)] int page = 1,
		[FromQuery, Range(1, 100)] int pageSize = 20
	)
	{
		return Ok(await service.GetPageAsync(page, pageSize, ct));
	}
}