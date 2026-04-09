namespace ContentAPI.Entities
{
    public class AiContent
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Prompt { get; set; } = string.Empty;
        public string? GeneratedText { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
