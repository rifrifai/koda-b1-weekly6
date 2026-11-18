namespace KodaEWallet.Exceptions;

public class InsufficientBalanceException()
	: AppException("Saldo tidak mencukupi", StatusCodes.Status422UnprocessableEntity)
{
}