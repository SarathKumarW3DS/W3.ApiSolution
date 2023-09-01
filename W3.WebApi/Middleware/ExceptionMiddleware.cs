using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using W3.Domain.Interfaces;
using W3.WebApi.Error;

namespace W3.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        private readonly ILoggerManager _logger1;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, ILoggerManager logger1,
            IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _logger1 = logger1;
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
                _logger.LogError(ex, ex.Message);

                context.Response.ContentType = "application/json";

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = _env.IsDevelopment()

                    ? new ApiError(context.Response.StatusCode, ex.Message,
                    ex.StackTrace?.ToString())

                    : new ApiError(context.Response.StatusCode, "Internal Server Error");

                var options = new JsonSerializerOptions
                { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

                var json = JsonSerializer.Serialize(response, options);

                await context.Response.WriteAsync(json);
                _logger1.LogError(json);
            }
        }
    }
}
