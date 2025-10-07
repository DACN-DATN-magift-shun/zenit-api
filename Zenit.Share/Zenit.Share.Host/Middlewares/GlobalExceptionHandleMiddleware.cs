using Microsoft.AspNetCore.Http;
using Zenit.Share.Common.Exceptions;

namespace Zenit.Share.Host.Middlewares
{
    public class GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

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

            var details = exception.Message;

            if (exception is HttpException httpException)
            {
                context.Response.StatusCode = (int)httpException.StatusCode;

                var httpResponse = new
                {
                    code = context.Response.StatusCode,
                    message = httpException.Message.ToString(),
                    details
                };

                return context.Response.WriteAsJsonAsync(httpResponse);
            }

            context.Response.Clear();

            var response = new
            {
                code = StatusCodes.Status500InternalServerError,
                message = "Internal Server Error",
                details
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
