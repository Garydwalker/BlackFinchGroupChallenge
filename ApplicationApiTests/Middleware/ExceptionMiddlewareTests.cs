using System.Net;
using ApplicationApi.MiddleWare;
using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace ApplicationApiTests.Middleware
{
    public class ExceptionMiddlewareTests
    {
        
        [Test]
        public async Task Middleware_ReturnsBadRequest_ForValidationException()
        {
            // Arrange
            RequestDelegate next = (HttpContext hc) => throw new ValidationException("validation Error");
            
            var context = new DefaultHttpContext();
            var middleware = new ExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
            context.Response.ContentType.Should().Be("application/json");
        }

        [Test]
        public async Task Middleware_ReturnsInternalServerError_ForGenericException()
        {
            // Arrange
            RequestDelegate next = (HttpContext hc) => throw new Exception("An error occurred");

            var context = new DefaultHttpContext();
            var middleware = new ExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
            context.Response.ContentType.Should().Be("application/json");
        }

        [Test]
        public async Task Middleware_PassesRequest_ToNextMiddleware_WhenNoException()
        {
            // Arrange
            RequestDelegate next = (HttpContext hc) => Task.CompletedTask;

            var context = new DefaultHttpContext();
            var middleware = new ExceptionMiddleware(next);

            // Act
            await middleware.InvokeAsync(context);

            // Assert
            context.Response.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
