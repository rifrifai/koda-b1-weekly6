using KodaEWallet.Models;

namespace KodaEWallet.Repositories;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync(CancellationToken ct);
    Task<User?> GetByIdAsync(long id, CancellationToken ct);
    Task<bool> ExistsAsync(long id, CancellationToken ct);
    Task<bool> IsDuplicateAsync(string? username, string email, string phone, long exceptUserId, CancellationToken ct);
    void Add(User user);
}