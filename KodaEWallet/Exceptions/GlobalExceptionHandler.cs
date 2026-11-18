using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace KodaEWallet.Exceptions;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
	{
		if (exception is OperationCanceledException)
        {
            httpContext.Response.StatusCode = 499;
            return true;
        }

        int status;
        string title;

        switch (exception)
        {
            case AppException app:
                status = app.StatusCode;
                title = app.Message;
                break;
            case DbUpdateConcurrencyException:
                status = StatusCodes.Status409Conflict;
                title = "Wallet sedang diubah oleh proses lain. Tidak ada perubahan yang disimpan, silakan coba lagi";
                break;
            case DbUpdateException { InnerException: SqlException sql } when sql.Number is 2627 or 2601:
                status = StatusCodes.Status409Conflict;
                title = "Data sudah ada (duplikat)";
                break;
            case DbUpdateException:
                status = StatusCodes.Status500InternalServerError;
                title = "Terjadi kesalahan pada database";
                break;
            default:
                status = StatusCodes.Status500InternalServerError;
                title = "Terjadi kesalahan pada server";
                break;
        }

        if (status >= 500)
        {
            logger.LogError(exception, "Unhandled exception pada {Path}", httpContext.Request.Path);
        }
        else
        {
            logger.LogWarning("{Message} ({Path})", exception.Message, httpContext.Request.Path);
        }

        var problem = new ProblemDetails
        {
            Status = status,
            Title = title,
            Instance = httpContext.Request.Path
        };

        httpContext.Response.StatusCode = status;
        await httpContext.Response.WriteAsJsonAsync(problem, cancellationToken);
        return true;
	}
}