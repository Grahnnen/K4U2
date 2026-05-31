using LLMProxy.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace LLMProxy.Middleware
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionMiddleware> _logger;

        public CustomExceptionMiddleware(
            RequestDelegate next,
            ILogger<CustomExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ExternalAiException ex)
            {
                _logger.LogWarning(
                    ex,
                    "External AI error. StatusCode: {StatusCode}, Title: {Title}",
                    (int)ex.StatusCode,
                    ex.Title);

                await WriteProblemDetailsAsync(
                    context,
                    (int)ex.StatusCode,
                    ex.Title,
                    ex.SafeDetail);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogWarning(ex, "The external AI request timed out or was cancelled.");

                await WriteProblemDetailsAsync(
                    context,
                    StatusCodes.Status504GatewayTimeout,
                    "AI request timeout",
                    "AI-tjänsten svarade inte i tid. Försök igen senare.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception.");

                await WriteProblemDetailsAsync(
                    context,
                    StatusCodes.Status500InternalServerError,
                    "Internal server error",
                    "Ett oväntat fel uppstod.");
            }
        }

        private static async Task WriteProblemDetailsAsync(
            HttpContext context,
            int statusCode,
            string title,
            string detail)
        {
            if (context.Response.HasStarted)
            {
                return;
            }

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/problem+json";

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = context.Request.Path
            };

            problemDetails.Extensions["traceId"] = context.TraceIdentifier;

            await context.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}