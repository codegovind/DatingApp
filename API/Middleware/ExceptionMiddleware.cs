using System.Drawing;
using System.Net;
using System.Text.Json;
using API.Errors;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,
         IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
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
                _logger.LogError(ex.Message);
                context.Response.ContentType="application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                var response = _env.IsDevelopment()          //use turnary expression
                ? new ApiException(context.Response.StatusCode,ex.Message,ex.StackTrace?.ToString()) //if true condition
                : new ApiException(context.Response.StatusCode,ex.Message,"Internal Server Error"); //if false condition
                var option = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(response, option);
                 
                 await context.Response.WriteAsync(json);
            }
        }
    }
}