using System.Text.Json.Serialization;

namespace LLMProxy.Models.OpenAi;

public class OpenAiGenerateResponse
{
    [JsonPropertyName("output")]
    public List<OpenAiOutputItem>? Output { get; set; }
}

public class OpenAiOutputItem
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("content")]
    public List<OpenAiOutputContent>? Content { get; set; }
}

public class OpenAiOutputContent
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("text")]
    public string? Text { get; set; }
}