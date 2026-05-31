namespace LLMProxy.Options
{
    public class OpenAiOptions
    {
        public string BaseUrl { get; set; } = "https://api.openai.com/";
        public string Endpoint { get; set; } = "v1/responses";
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gpt-5.4";
        public int TimeoutSeconds { get; set; } = 30;
    }
}
