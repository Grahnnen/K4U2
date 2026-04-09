using ContentAPI.Data;
using ContentAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContentAPI.Repositories
{
    public class AiContentRepository : IAiContentRepository
    {
        private readonly AppDbContext _context;

        public AiContentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AiContent>> GetAllAsync(string? category, DateTime? startDate, DateTime? endDate, string? sort)
        {
            var query = _context.Contents.AsQueryable();

            if (!string.IsNullOrWhiteSpace(category))
            {
                query = query.Where(x => x.Category.ToLower() == category.ToLower());
            }

            if (startDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(x => x.CreatedAt <= endDate.Value);
            }

            query = sort?.ToLower() switch
            {
                "createdat" => query.OrderBy(x => x.CreatedAt),
                "-createdat" => query.OrderByDescending(x => x.CreatedAt),
                "title" => query.OrderBy(x => x.Title),
                "-title" => query.OrderByDescending(x => x.Title),
                _ => query.OrderByDescending(x => x.CreatedAt)
            };

            return await query.ToListAsync();
        }

        public Task<AiContent?> GetByIdAsync(int id)
        {
            return _context.Contents.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AiContent> AddAsync(AiContent entity)
        {
            _context.Contents.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(AiContent entity)
        {
            var existing = await _context.Contents.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existing is null)
                return false;

            existing.Title = entity.Title;
            existing.Category = entity.Category;
            existing.Prompt = entity.Prompt;
            existing.GeneratedText = entity.GeneratedText;
            existing.UpdatedAt = entity.UpdatedAt;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Contents.FirstOrDefaultAsync(x => x.Id == id);
            if (existing is null)
                return false;

            _context.Contents.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }

        public Task SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}
