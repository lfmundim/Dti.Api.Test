using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using Serilog;

namespace Dti.Api.Test.Middleware
{
    /// <summary>
    /// Wraps all controller actions with a try-catch latch to avoid code repetition
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public ErrorHandlingMiddleware(RequestDelegate next, ILogger logger)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invoke Method, to validate requisition errors
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
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

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is ArgumentException argumentException)
            {
                _logger.Error(argumentException, "Error: {@exception}", exception.Message);
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
            else
            {
                _logger.Error(exception, "Error: {@exception}", exception.Message);
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            }

            string body = string.Empty;
            using (var reader = new StreamReader(context.Request.Body))
            {
                body = await reader.ReadToEndAsync();
            }

            _logger.Error(exception, "[traceId:{@traceId}] Error. Headers: {@headers}. Query: {@query}. Path: {@path}. Body: {@body}",
                          context.TraceIdentifier, context.Request.Headers, context.Request.Query, context.Request.Path, body);

            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(JsonConvert.SerializeObject($"{exception.Message}| traceId: {context.TraceIdentifier}"));
        }
    }
}
