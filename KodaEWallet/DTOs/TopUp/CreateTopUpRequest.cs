using System.ComponentModel.DataAnnotations;

namespace KodaEWallet.DTOs.TopUp;

public class CreateTopUpRequest
{
	[Range(0.01, 1_000_000_000, ErrorMessage = "Nominal harus lebih dari 0 dan maksimal Rp1.000.000.000")]
	public decimal Amount { get; init; }

	[Required(ErrorMessage = "Kode metode pembayaran wajib diisi")]
	[StringLength(30, ErrorMessage = "Kode metode pembayaran maksimal 30 karakter")]
	public string PaymentMethodCode { get; init; } = "";

	[StringLength(200, ErrorMessage = "Keterangan maksimal 200 karakter")]
	public string? Description { get; init; }
}
