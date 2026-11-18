using System.Diagnostics;

namespace KodaEWallet.Models;

public sealed class AuditLog
{
	private AuditLog() { }

	public static AuditLog Create(string action, string? entityId, string outcome,
		string description, long? actorUserId, Transaction? transaction) => new()
	{
		ActorUserId = actorUserId,
		Transaction = transaction,
		Action = action,
		EntityType = "Transaction",
		EntityId = entityId,
		Outcome = outcome,
		Description = description,
		CorrelationId = Activity.Current?.TraceId.ToString(),
		CreatedAtUtc = DateTime.UtcNow
	};

	public long Id { get; private set; }
	public long? ActorUserId { get; private set; }
	public long? TransactionId { get; private set; }
	public string Action { get; private set; } = "";
	public string EntityType { get; private set; } = "";
	public string? EntityId { get; private set; }
	public string Outcome { get; private set; } = "";
	public string? Description { get; private set; }
	public string? CorrelationId { get; private set; }
	public DateTime CreatedAtUtc { get; private set; }

	public Transaction? Transaction { get; private set; }
}