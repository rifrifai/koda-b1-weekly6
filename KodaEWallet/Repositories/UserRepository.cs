using KodaEWallet.Data;
using KodaEWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace KodaEWallet.Repositories;

public class UserRepository(AppDbContext db) : IUserRepository
{
	public void Add(User user) => db.Users.Add(user);
	public Task<bool> ExistsAsync(long id, CancellationToken ct) => db.Users.AnyAsync(u => u.Id == id, ct);
	public Task<List<User>> GetAllAsync(CancellationToken ct) => db.Users.AsNoTracking().OrderBy(u => u.Id).ToListAsync(ct);
	public Task<User?> GetByIdAsync(long id, CancellationToken ct) => db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);
	public Task<bool> IsDuplicateAsync(string? username, string email, string phone, long exceptUserId, CancellationToken ct) =>
		db.Users.AnyAsync(u => u.Id != exceptUserId && (u.Username == username || u.Email == email || u.Phone == phone), ct);
}
