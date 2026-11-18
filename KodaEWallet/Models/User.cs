namespace KodaEWallet.Models;

public class User
{
	private User() { }

	public static User Create(string username, string fullName, string email, string phone) => new()
	{
		Username = username,
		FullName = fullName,
		Email = email,
		Phone = phone,
		Status = UserStatus.Active,
		CreatedAtUtc = DateTime.UtcNow
	};

	public long Id { get; private set; }
	public string Username { get; private set; } = "";
	public string FullName { get; private set; } = "";
	public string Email { get; private set; } = "";
	public string Phone { get; private set; } = "";
	public string? PasswordHash { get; private set; } 
	public string Status { get; private set; } = UserStatus.Active;
	public DateTime CreatedAtUtc { get; private set; }
	public DateTime? UpdatedAtUtc { get; private set; }

	public void UpdateProfile(string fullName, string email, string phone)
	{
		FullName = fullName;
		Email = email;
		Phone = phone;
		UpdatedAtUtc = DateTime.UtcNow;
	}
}
