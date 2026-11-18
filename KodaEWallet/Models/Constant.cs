namespace KodaEWallet.Models;

public static class UserStatus
{
	public const string Active = "ACTIVE";
	public const string Suspended = "SUSPENDED";
}

public static class WalletStatus
{
	public const string Active = "ACTIVE";
	public const string Frozen = "FROZEN";
	public const string Closed = "CLOSED";
}

public static class TransactionType
{
	public const string TopUp = "TOPUP";
	public const string Transfer = "TRANSFER";
}

public static class TransactionStatus
{
	public const string Pending = "PENDING";
	public const string Success = "SUCCESS";
	public const string Failed = "FAILED";
}

public static class MutationDirection
{
	public const string In = "IN";
	public const string Out = "OUT";
}

public static class TopUpStatus
{
	public const string Pending = "PENDING";
	public const string Paid = "PAID";
	public const string Failed = "FAILED";
	public const string Expired = "EXPIRED";
}

public static class AuditAction
{
	public const string TopUpRequested = "TOPUP_REQUESTED";
	public const string TopUpPaid = "TOPUP_PAID";
	public const string TransferCreated = "TRANSFER_CREATED";
	public const string TransferRejected = "TRANSFER_REJECTED";
}

public static class AuditOutcome
{
	public const string Success = "SUCCESS";
	public const string Rejected = "REJECTED";
	public const string Failed = "FAILED";
}
