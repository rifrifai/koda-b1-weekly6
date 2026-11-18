using System.ComponentModel.DataAnnotations;

namespace KodaEWallet.DTOs.Payment;

public class TransferRequest
{
    [Range(1, long.MaxValue, ErrorMessage = "Sender User Id tidak valid")]
    public long SenderUserId { get; init; }

    [Required(ErrorMessage = "Nomor wallet tujuan wajib diisi")]
    [StringLength(30, ErrorMessage = "Nomor wallet maksimal 30 karakter")]
    public string ReceiverWalletNumber { get; init; } = "";

    [Range(0.01, 1_000_000_000, ErrorMessage = "Nominal harus lebih dari 0 dan maksimal Rp1.000.000.000")]
    public decimal Amount { get; init; }

    [StringLength(200, ErrorMessage = "Keterangan maksimal 200 karakter")]
    public string? Description { get; init; }
}