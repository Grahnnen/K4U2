namespace LLMProxy.Options
{
    public class OpenAiOptions
    {
        public string BaseUrl { get; set; } = "https://api.openai.com/";
        public string Endpoint { get; set; } = "v1/responses";
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gpt-5.4";
        public int TimeoutSeconds { get; set; } = 30;

        public string DeveloperMessage { get; set; } = """
        You are an AI content assistant.

        Rules:
        - Answer in the same language as the user prompt.
        - Keep the answer clear and useful.
        - Do not invent facts, numbers, customers, awards or sources.
        - If you are unsure, say that you are unsure.
        - Do not include API keys, internal errors or system details.
        - Keep the answer reasonably short unless the user asks for a longer text.

        Preferred format:
        Title:
        Text:
        Uncertainty:
        """;
    }
}
