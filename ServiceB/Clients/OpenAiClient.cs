using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace LLMProxy.Clients
{
    public class OpenAiClient
    {
        private readonly HttpClient _httpClient;

        public OpenAiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GenerateAsync(string prompt, string model, CancellationToken ct = default)
        {
            var response = await _httpClient.PostAsJsonAsync("v1/responses", new
            {
                model,
                input = prompt
            }, ct);

            var body = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"OpenAI error {(int)response.StatusCode}: {body}");
            }

            var result = JsonSerializer.Deserialize<OpenAiResponse>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result?.Output is null || result.Output.Count == 0)
            {
                return string.Empty;
            }

            var texts = result.Output
                .Where(o => string.Equals(o.Type, "message", StringComparison.OrdinalIgnoreCase))
                .SelectMany(o => o.Content ?? new List<OutputContent>())
                .Where(c => string.Equals(c.Type, "output_text", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Text)
                .Where(t => !string.IsNullOrWhiteSpace(t));

            return string.Join("\n", texts);
        }

        private sealed class OpenAiResponse
        {
            [JsonPropertyName("output")]
            public List<OutputItem>? Output { get; set; }
        }

        private sealed class OutputItem
        {
            [JsonPropertyName("type")]
            public string? Type { get; set; }

            [JsonPropertyName("content")]
            public List<OutputContent>? Content { get; set; }
        }

        private sealed class OutputContent
        {
            [JsonPropertyName("type")]
            public string? Type { get; set; }

            [JsonPropertyName("text")]
            public string? Text { get; set; }
        }
    }
}