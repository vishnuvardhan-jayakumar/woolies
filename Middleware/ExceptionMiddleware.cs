using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Woolies.Model.Exception;

#pragma warning disable 4014

namespace Woolies.Middleware
{
    internal class ExceptionMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<ExceptionMiddleware>();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is ValidationException)
            {
                var problemDetails = new ValidationProblemDetails
                {
                    Instance = context.Request.Path,
                    Status = StatusCodes.Status400BadRequest,
                    Detail = "Request validation failed"
                };

                problemDetails.Errors.Add("DomainValidations", new[] {exception.Message});

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.BadRequest;

                return context.Response.WriteAsync(JsonConvert.SerializeObject(problemDetails));
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            var responseMessage = new
            {
                Message = "Request could not be processed."
            };
            
            return context.Response.WriteAsync(JsonConvert.SerializeObject(responseMessage));
        }
    }
}