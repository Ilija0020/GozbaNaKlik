using gozba_na_klik_backend.Exceptions;
using System.Text.Json;

namespace gozba_na_klik_backend.Middleware
{
    internal sealed class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(ILogger<ExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(context, e);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = exception switch
            {
                BadRequestException => StatusCodes.Status400BadRequest,
                ForbiddenException => StatusCodes.Status403Forbidden,
                NotFoundException => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError
            };

            if (httpContext.Response.StatusCode == StatusCodes.Status500InternalServerError)
                _logger.LogError(exception, "Neocekivana greska prilikom obrade zahteva.");

            var errorResponse = new ErrorResponse
            {
                StatusCode = httpContext.Response.StatusCode,
                Message = httpContext.Response.StatusCode == StatusCodes.Status500InternalServerError
                    ? "Doslo je do greske na serveru."
                    : exception.Message
            };

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }
}