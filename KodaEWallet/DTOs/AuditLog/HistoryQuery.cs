using System.ComponentModel.DataAnnotations;

namespace KodaEWallet.DTOs.AuditLog;

public class HistoryQuery
{
	[StringLength(50)]
	public string? Search { get; init; }

	[RegularExpression("^(TOPUP|TRANSFER)$", ErrorMessage = "Type harus TOPUP atau TRANSFER")]
	public string? Type { get; init; }

	[RegularExpression("^(PENDING|SUCCESS|FAILED)$", ErrorMessage = "Status harus PENDING, SUCCESS, atau FAILED")]
	public string? Status { get; init; }

	[RegularExpression("^(IN|OUT)$", ErrorMessage = "Direction harus IN atau OUT")]
	public string? Direction { get; init; }

	public DateTime? DateFrom { get; init; }
	public DateTime? DateTo { get; init; }

	[RegularExpression("^(createdAt|amount)$", ErrorMessage = "SortBy harus createdAt atau amount")]
	public string SortBy { get; init; } = "createdAt";

	[RegularExpression("^(asc|desc)$", ErrorMessage = "SortDir harus asc atau desc")]
	public string SortDir { get; init; } = "desc";

	[Range(1, int.MaxValue, ErrorMessage = "Page minimal 1")]
	public int Page { get; init; } = 1;

	[Range(1, 100, ErrorMessage = "PageSize 1-100")]
	public int PageSize { get; init; } = 10;
}