using ContentAPI.Entities;

namespace ContentAPI.Repositories
{
    public interface IAiContentRepository
    {
        Task<List<AiContent>> GetAllAsync(string? category, DateTime? startDate, DateTime? endDate, string? sort);
        Task<AiContent?> GetByIdAsync(int id);
        Task<AiContent> AddAsync(AiContent entity);
        Task<bool> UpdateAsync(AiContent entity);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
