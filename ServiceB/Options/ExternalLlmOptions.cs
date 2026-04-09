namespace LLMProxy.Options
{
    public class ExternalLlmOptions
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string Model { get; set; } = "gemma3";
        public string? ApiKey { get; set; }
    }
}
