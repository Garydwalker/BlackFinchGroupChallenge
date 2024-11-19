using System.Net;
using System.Text.Json;
using FluentValidation;

namespace ApplicationApi.MiddleWare
{
    public class ExceptionMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (Exception)
            {
                await HandleExceptionAsync(context);
            }
        }

        private static Task HandleValidationExceptionAsync(HttpContext context, ValidationException exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errors = exception.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });

            var result = JsonSerializer.Serialize(new { errors });
            return context.Response.WriteAsync(result);
        }

        private static Task HandleExceptionAsync(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            
            return context.Response.WriteAsync(string.Empty);
        }
    }
}
