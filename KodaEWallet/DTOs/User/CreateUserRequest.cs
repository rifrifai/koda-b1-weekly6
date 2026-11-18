using System.ComponentModel.DataAnnotations;

namespace KodaEWallet.DTOs.User;

public class CreateUserRequest
{
    [Required(ErrorMessage = "Username wajib diisi")]
    [RegularExpression(@"^[a-zA-Z0-9_.]{3,50}$", ErrorMessage = "Username 3-50 karakter: huruf, angka, titik, atau underscore")]
    public string Username { get; init; } = "";

    [Required(ErrorMessage = "Nama lengkap wajib diisi")]
    [StringLength(100, ErrorMessage = "Nama maksimal 100 karakter")]
    public string FullName { get; init; } = "";

    [Required(ErrorMessage = "Email wajib diisi")]
    [EmailAddress(ErrorMessage = "Format email tidak valid")]
    [StringLength(150, ErrorMessage = "Email maksimal 150 karakter")]
    public string Email { get; init; } = "";

    [Required(ErrorMessage = "Nomor telepon wajib diisi")]
    [RegularExpression(@"^[0-9]{8,15}$", ErrorMessage = "Nomor telepon harus 8-15 digit angka")]
    public string Phone { get; init; } = "";
}
