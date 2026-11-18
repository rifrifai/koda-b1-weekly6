using KodaEWallet.DTOs.User;

namespace KodaEWallet.Services;

public interface IUserService
{
	Task<List<UserResponse>> GetAllAsync(CancellationToken ct);
	Task<UserResponse> GetByIdAsync(long id, CancellationToken ct);
	Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct);
	Task<UserResponse> UpdateAsync(long id, UpdateUserRequest request, CancellationToken ct);
}