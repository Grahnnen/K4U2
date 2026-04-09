using System.ComponentModel.DataAnnotations;

namespace ContentAPI.DTOs
{
    public class CreateAiContentRequest
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [Required]
        [StringLength(4000)]
        public string Prompt { get; set; } = string.Empty;
    }
}
