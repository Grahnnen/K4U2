using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Json;
using WebApplication1.Exceptions;

namespace ContentApi.Clients;

public class LlmProxyClient
{
    private readonly HttpClient _httpClient;

    public LlmProxyClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/api/llm/generate",
            new { prompt },
            ct);

        var body = await response.Content.ReadAsStringAsync(ct);

        if (!response.IsSuccessStatusCode)
        {
            ProblemDetails? problemDetails = null;

            try
            {
                problemDetails = await response.Content.ReadFromJsonAsync<ProblemDetails>(cancellationToken: ct);
            }
            catch
            {
                // If ServiceB did not return valid ProblemDetails, we still return a safe error.
            }

            var title = problemDetails?.Title ?? "LLM proxy error";
            var detail = problemDetails?.Detail ?? "AI-tjänsten kunde inte slutföra begäran.";

            throw new DownstreamServiceException(
                response.StatusCode,
                title,
                detail);
        }

        var result = await response.Content.ReadFromJsonAsync<GenerateResponse>(cancellationToken: ct);

        return result?.GeneratedText ?? string.Empty;
    }

    private class GenerateResponse
    {
        public string GeneratedText { get; set; } = string.Empty;
    }
}