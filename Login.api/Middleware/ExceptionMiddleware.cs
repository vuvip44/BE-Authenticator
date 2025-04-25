using Login.api.Common;
using Login.api.Middleware.Exceptions;
using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace Login.api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var statusCode = (int)HttpStatusCode.InternalServerError; // Default = 500

            switch (exception)
            {
                case AppException:
                    statusCode = (int)HttpStatusCode.BadRequest; // 400
                    break;
                case UnauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized; // 401
                    break;
                case ForbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden; // 403
                    break;
                case KeyNotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound; // 404
                    break;

            }

            context.Response.StatusCode = statusCode;

            var response = new ApiResponse<string>(statusCode, exception.Message);

            var result = JsonSerializer.Serialize(response);

            return context.Response.WriteAsync(result);
        }

    }
}
