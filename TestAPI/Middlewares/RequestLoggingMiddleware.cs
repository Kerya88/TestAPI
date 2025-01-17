using TestAPI.Contexts;
using TestAPI.Entities;

namespace TestAPI.Middlewares
{
    public class RequestLoggingMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context, ToDoTasksContext dbContext)
        {
            var requestLog = new RequestLog
            {
                IPAddress = context.Connection.RemoteIpAddress!.ToString(),
                Path = context.Request.Path,
                Date = DateTime.UtcNow
            };

            await dbContext.RequestLogs.AddAsync(requestLog);
            await dbContext.SaveChangesAsync();

            await _next(context);
        }
    }
}
