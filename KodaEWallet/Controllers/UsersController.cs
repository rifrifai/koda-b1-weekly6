using KodaEWallet.DTOs;
using KodaEWallet.DTOs.AuditLog;
using KodaEWallet.DTOs.Transaction;
using KodaEWallet.DTOs.User;
using KodaEWallet.DTOs.WalletMutation;
using KodaEWallet.Services;
using Microsoft.AspNetCore.Mvc;

namespace KodaEWallet.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class UsersController(IUserService userService, IWalletService walletService) : ControllerBase
{
	[HttpGet]
	public async Task<ActionResult<List<UserResponse>>> GetAll(CancellationToken ct)
	{
		return Ok(await userService.GetAllAsync(ct));
	}

	[HttpGet("{id:long}")]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<UserResponse>> GetById(long id, CancellationToken ct)
	{
		return Ok(await userService.GetByIdAsync(id, ct));
	}

	[HttpPost]
	[ProducesResponseType(typeof(UserResponse), StatusCodes.Status201Created)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
	public async Task<ActionResult<UserResponse>> Create(CreateUserRequest request, CancellationToken ct)
	{
		UserResponse user = await userService.CreateAsync(request, ct);
		return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
	}

	[HttpPut("{id:long}")]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
	public async Task<ActionResult<UserResponse>> Update(long id, UpdateUserRequest request, CancellationToken ct)
	{
		return Ok(await userService.UpdateAsync(id, request, ct));
	}

	[HttpGet("{id:long}/transactions")]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<PagedResponse<TransactionResponse>>> GetTransactions(
		long id, [FromQuery] HistoryQuery query, CancellationToken ct)
	{
		return Ok(await walletService.GetTransactionsAsync(id, query, ct));
	}

	[HttpGet("{id:long}/mutations")]
	[ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
	public async Task<ActionResult<PagedResponse<MutationResponse>>> GetMutations(
		long id, [FromQuery] HistoryQuery query, CancellationToken ct)
	{
		return Ok(await walletService.GetMutationsAsync(id, query, ct));
	}
}