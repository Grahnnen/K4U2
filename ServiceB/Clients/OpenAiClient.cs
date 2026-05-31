using LLMProxy.Exceptions;
using LLMProxy.Models.OpenAi;
using LLMProxy.Options;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace LLMProxy.Clients
{
    public class OpenAiClient
    {
        private readonly HttpClient _httpClient;
        private readonly OpenAiOptions _options;

        public OpenAiClient(HttpClient httpClient, IOptions<OpenAiOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
        {
            var request = new OpenAiGenerateRequest
            {
                Model = _options.Model,
                Instructions = _options.DeveloperMessage,
                Input = prompt
            };

            var response = await _httpClient.PostAsJsonAsync(
                _options.Endpoint,
                request,
                ct);

            var body = await response.Content.ReadAsStringAsync(ct);

            if (!response.IsSuccessStatusCode)
            {
                throw response.StatusCode switch
                {
                    HttpStatusCode.Unauthorized => new ExternalAiException(
                        HttpStatusCode.BadGateway,
                        "AI authentication failed",
                        "AI-tjänsten kunde inte autentiseras. Kontrollera API-nyckeln."),

                    HttpStatusCode.Forbidden => new ExternalAiException(
                        HttpStatusCode.BadGateway,
                        "AI access denied",
                        "AI-tjänsten nekade åtkomst."),

                    (HttpStatusCode)429 => new ExternalAiException(
                        (HttpStatusCode)429,
                        "AI rate limit reached",
                        "AI-tjänsten är tillfälligt överbelastad. Försök igen senare."),

                    HttpStatusCode.BadRequest => new ExternalAiException(
                        HttpStatusCode.BadRequest,
                        "Invalid AI request",
                        "Prompten eller AI-förfrågan var ogiltig."),

                    _ when (int)response.StatusCode >= 500 => new ExternalAiException(
                        HttpStatusCode.BadGateway,
                        "AI provider error",
                        "AI-tjänsten svarade med ett serverfel."),

                    _ => new ExternalAiException(
                        HttpStatusCode.BadGateway,
                        "AI provider error",
                        "Ett oväntat fel uppstod vid anrop till AI-tjänsten.")
                };
            }

            var result = JsonSerializer.Deserialize<OpenAiGenerateResponse>(
                body,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result?.Output is null || result.Output.Count == 0)
            {
                throw new ExternalAiException(HttpStatusCode.BadGateway, "Empty AI response", "AI-tjänsten returnerade ett tomt svar.");
            }

            var texts = result.Output
                .Where(o => string.Equals(o.Type, "message", StringComparison.OrdinalIgnoreCase))
                .SelectMany(o => o.Content ?? new List<OpenAiOutputContent>())
                .Where(c => string.Equals(c.Type, "output_text", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Text)
                .Where(t => !string.IsNullOrWhiteSpace(t));

            var generatedText = string.Join("\n", texts);

            if (string.IsNullOrWhiteSpace(generatedText))
            {
                throw new ExternalAiException(HttpStatusCode.BadGateway, "Unexpected AI response", "AI-tjänsten returnerade ett svar i oväntat format.");
            }

            return generatedText;
        }
    }
}