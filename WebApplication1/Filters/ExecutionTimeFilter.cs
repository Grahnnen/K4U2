using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace ContentAPI.Filters
{
    public class ExecutionTimeFilter : IAsyncActionFilter
    {
        private readonly ILogger<ExecutionTimeFilter> _logger;

        public ExecutionTimeFilter(ILogger<ExecutionTimeFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var stopwatch = Stopwatch.StartNew();

            var executedContext = await next();

            stopwatch.Stop();

            var elapsedMs = stopwatch.ElapsedMilliseconds;

            context.HttpContext.Response.Headers["X-Execution-Time-Ms"] = elapsedMs.ToString();

            _logger.LogInformation(
                "Action {ActionName} executed in {ElapsedMs} ms",
                context.ActionDescriptor.DisplayName,
                elapsedMs);
        }
    }
}
