using System.Net.Http.Json;

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
            throw new Exception($"ServiceB error {(int)response.StatusCode}: {body}");
        }

        var result = await response.Content.ReadFromJsonAsync<GenerateResponse>(cancellationToken: ct);

        return result?.GeneratedText ?? string.Empty;
    }

    private class GenerateResponse
    {
        public string GeneratedText { get; set; } = string.Empty;
    }
}