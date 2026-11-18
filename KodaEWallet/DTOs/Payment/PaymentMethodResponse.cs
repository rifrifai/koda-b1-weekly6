namespace KodaEWallet.DTOs.Payment;

public sealed class PaymentMethodResponse
{
	public int Id { get; init; }
	public string Code { get; init; } = "";
	public string Name { get; init; } = "";
	public string? ProviderName { get; init; }
}