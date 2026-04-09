using ContentAPI.DTOs;

namespace ContentAPI.Services
{
    public interface IAiContentService
    {
        Task<List<AiContentResponse>> GetAllAsync(string? category, DateTime? startDate, DateTime? endDate, string? sort);
        Task<AiContentResponse?> GetByIdAsync(int id);
        Task<AiContentResponse> CreateAsync(CreateAiContentRequest request);
        Task<bool> UpdateAsync(int id, UpdateAiContentRequest request);
        Task<bool> DeleteAsync(int id);
        Task<AiContentResponse?> GenerateAsync(int id, CancellationToken ct = default);
    }
}
