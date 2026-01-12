using BankingApi.Api.Contracts.Common;
using BankingApi.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace BankingApi.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

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

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ApiResponse<Object>
            {
                Success = false
            };

            switch (exception)
            {
                case InsufficientFundsException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Errors.Add(
                        new ApiError
                        {
                            Code = "INSUFFICIENT_FUNDS",
                            Message = exception.Message
                        }
                    );
                    break;

                case ArgumentException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Errors.Add(
                        new ApiError
                        {
                            Code = "INVALID_REQUEST",
                            Message = exception.Message
                        }                        
                    );
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Errors.Add(
                        new ApiError
                        {
                            Code = "INTERNAL_SERVER_ERROR",
                            Message = "An unexpected error occurred"
                        }    
                    );
                    break;
            }

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
