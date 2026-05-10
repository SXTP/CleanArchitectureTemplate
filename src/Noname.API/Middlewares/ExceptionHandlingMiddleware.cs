using Noname.Application.Common.Exceptions;
using Noname.Application.Common.Models;

namespace Noname.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, errors) = exception switch
        {
            CustomValidationException validationEx => (StatusCodes.Status400BadRequest, validationEx.Errors),
            _ => (StatusCodes.Status500InternalServerError, new[] { "An unexpected error occurred." })
        };

        context.Response.StatusCode = statusCode;

        var result = Result.Failure(errors);
        return context.Response.WriteAsJsonAsync(result);
    }
}