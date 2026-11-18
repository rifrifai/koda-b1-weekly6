namespace KodaEWallet.DTOs.User;

public class UserResponse
{
	public long Id { get; init; }
	public string Username { get; init; } = "";
	public string FullName { get; init; } = "";
	public string Email { get; init; } = "";
	public string Phone { get; init; } = "";
	public string Status { get; init; } = "";
}
