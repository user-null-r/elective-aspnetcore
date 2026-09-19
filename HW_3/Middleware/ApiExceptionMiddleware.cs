using HW_3.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace HW_3.Middleware;

public sealed class ApiExceptionMiddleware(RequestDelegate next, ILogger<ApiExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Ошибка обработки запроса");
            var (statusCode, message) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                ConflictException => (StatusCodes.Status409Conflict, exception.Message),
                DbUpdateException => (StatusCodes.Status409Conflict, "Пользователь с такими данными уже существует."),
                _ => (StatusCodes.Status500InternalServerError, "Внутренняя ошибка сервера.")
            };
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(new { error = message });
        }
    }
}

