using KodaEWallet.DTOs.User;
using KodaEWallet.Exceptions;
using KodaEWallet.Models;
using KodaEWallet.Repositories;

namespace KodaEWallet.Services;

public class UserService(IUserRepository users, IWalletRepository wallets, IUnitOfWork uow) : IUserService
{
    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct)
    {
        string username = request.Username.Trim();
        string email = request.Email.Trim();
        string phone = request.Phone.Trim();

        if (await users.IsDuplicateAsync(username, email, phone, 0, ct))
        {
            throw new ConflictException("Username, email, atau nomor telepon sudah digunakan");
        }

        await using var tx = await uow.BeginTransactionAsync(ct);

        var user = User.Create(username, request.FullName.Trim(), email, phone);
        users.Add(user);
        await uow.SaveChangesAsync(ct);

        wallets.Add(Wallet.Create(user.Id, $"WAL{user.Id:D3}"));
        await uow.SaveChangesAsync(ct);

        await tx.CommitAsync(ct);
        return ToResponse(user);
    }

    public async Task<List<UserResponse>> GetAllAsync(CancellationToken ct)
    {
        List<User> all = await users.GetAllAsync(ct);
        return all.Select(ToResponse).ToList();
    }

    public async Task<UserResponse> GetByIdAsync(long id, CancellationToken ct)
    {
        User user = await users.GetByIdAsync(id, ct) ?? throw new NotFoundException($"User dengan id {id} tidak ditemukan");
        return ToResponse(user);
    }

    public async Task<UserResponse> UpdateAsync(long id, UpdateUserRequest request, CancellationToken ct)
    {
        User user = await users.GetByIdAsync(id, ct) ?? throw new NotFoundException($"User dengan id {id} tidak ditemukan");

        string email = request.Email.Trim();
        string phone = request.Phone.Trim();

        if (await users.IsDuplicateAsync(null, email, phone, id, ct))
        {
            throw new ConflictException("Email atau nomor telepon sudah digunakan");
        }

        user.UpdateProfile(request.FullName.Trim(), email, phone);
        await uow.SaveChangesAsync(ct);
        return ToResponse(user);
    }

    private static UserResponse ToResponse(User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        FullName = user.FullName,
        Email = user.Email,
        Phone = user.Phone,
        Status = user.Status
    };
}