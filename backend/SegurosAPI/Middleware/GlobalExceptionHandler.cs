using System.Net;
using System.Text.Json;
using SegurosAPI.Exceptions;

namespace SegurosAPI.Middleware
{
    /// <summary>
    /// Middleware para manejo global de excepciones
    /// </summary>
    public class GlobalExceptionHandler
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(RequestDelegate next, ILogger<GlobalExceptionHandler> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;
            var message = "An internal server error occurred";

            switch (exception)
            {
                case NotFoundException:
                    code = HttpStatusCode.NotFound;
                    message = exception.Message;
                    break;
                case BusinessException:
                    code = HttpStatusCode.BadRequest;
                    message = exception.Message;
                    break;
            }

            var result = JsonSerializer.Serialize(new
            {
                success = false,
                message = message,
                error = exception.Message
            });

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(result);
        }
    }
}
