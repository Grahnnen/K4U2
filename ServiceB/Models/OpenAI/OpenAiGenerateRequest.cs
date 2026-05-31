using System.Text.Json.Serialization;

namespace LLMProxy.Models.OpenAi;

public class OpenAiGenerateRequest
{
    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("instructions")]
    public string Instructions { get; set; } = string.Empty;

    [JsonPropertyName("input")]
    public string Input { get; set; } = string.Empty;
}