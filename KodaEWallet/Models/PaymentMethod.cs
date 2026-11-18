namespace KodaEWallet.Models;

public class PaymentMethod
{
	private PaymentMethod() { }

	public static PaymentMethod Create(string code, string name, string? providerName) => new()
	{
		Code = code,
		Name = name,
		ProviderName = providerName,
		IsActive = true,
		CreatedAtUtc = DateTime.UtcNow
	};

	public int Id { get; private set; }
	public string Code { get; private set; } = "";
	public string Name { get; private set; } = "";
	public string? ProviderName { get; private set; }
	public bool IsActive { get; private set; }
	public DateTime CreatedAtUtc { get; private set; }
}