namespace KodaEWallet.DTOs.AuditLog;

public class AuditLogResponse
{
	public long Id { get; init; }
	public long? ActorUserId { get; init; }
	public long? TransactionId { get; init; }
	public string Action { get; init; } = "";
	public string EntityType { get; init; } = "";
	public string? EntityId { get; init; }
	public string Outcome { get; init; } = "";
	public string? Description { get; init; }
	public string? CorrelationId { get; init; }
	public DateTime CreatedAtUtc { get; init; }
}
