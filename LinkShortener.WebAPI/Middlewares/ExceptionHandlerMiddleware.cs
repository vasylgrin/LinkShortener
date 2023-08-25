using LinkShortener.Entity.Exceptions;
using System.Net;
using System.Text.Json;

namespace LinkShortener.WebAPI.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ArgumentNullException ex)
            {
                await HandleExceptionAsync(httpContext, ex.Message, "The input data is null.", HttpStatusCode.BadRequest);
            }
            catch (InvalidInputDataException ex)
            {
                await HandleExceptionAsync(httpContext, ex.Message, "Incorrect input data.", HttpStatusCode.BadRequest);
            }
            catch (ShortLinkNotFound ex)
            {
                await HandleExceptionAsync(httpContext, ex.Message, "Short link not found.", HttpStatusCode.BadRequest);
            }
            catch (UserExistException ex)
            {
                await HandleExceptionAsync(httpContext, ex.Message, "This user has already exists.", HttpStatusCode.BadRequest);
            }

            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex.Message, "Unknown error.", HttpStatusCode.BadRequest);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, string exMsg, string message, HttpStatusCode statusCode)
        {
            _logger.LogError(exMsg, message, statusCode);

            HttpResponse response = httpContext.Response;
            response.ContentType = "aplication/json";
            response.StatusCode = (int)statusCode;

            var error = new
            {
                exMessage = exMsg,
                defaultMessage = message,
                StatusCode = (int)statusCode
            };

            string result = JsonSerializer.Serialize(error);

            await response.WriteAsync(result);
        }
    }
}
