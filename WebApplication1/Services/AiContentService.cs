using ContentApi.Clients;
using ContentAPI.DTOs;
using ContentAPI.Entities;
using ContentAPI.Repositories;

namespace ContentAPI.Services
{
    public class AiContentService : IAiContentService
    {
        private readonly IAiContentRepository _repository;
        private readonly LlmProxyClient _llmProxyClient;

        public AiContentService(IAiContentRepository repository, LlmProxyClient llmProxyClient)
        {
            _repository = repository;
            _llmProxyClient = llmProxyClient;
        }

        public async Task<List<AiContentResponse>> GetAllAsync(string? category, DateTime? startDate, DateTime? endDate, string? sort)
        {
            var items = await _repository.GetAllAsync(category, startDate, endDate, sort);
            return items.Select(MapToResponse).ToList();
        }

        public async Task<AiContentResponse?> GetByIdAsync(int id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : MapToResponse(entity);
        }

        public async Task<AiContentResponse> CreateAsync(CreateAiContentRequest request)
        {
            var now = DateTime.UtcNow;

            var entity = new AiContent
            {
                Title = request.Title,
                Category = request.Category,
                Prompt = request.Prompt,
                CreatedAt = now,
                UpdatedAt = now
            };

            var created = await _repository.AddAsync(entity);
            return MapToResponse(created);
        }

        public async Task<bool> UpdateAsync(int id, UpdateAiContentRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return false;

            existing.Title = request.Title;
            existing.Category = request.Category;
            existing.Prompt = request.Prompt;
            existing.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateAsync(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        public async Task<AiContentResponse?> GenerateAsync(int id, CancellationToken ct = default)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
                return null;

            var generatedText = await _llmProxyClient.GenerateAsync(existing.Prompt, ct);

            existing.GeneratedText = generatedText;
            existing.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(existing);

            return MapToResponse(existing);
        }

        private static AiContentResponse MapToResponse(AiContent entity)
        {
            return new AiContentResponse
            {
                Id = entity.Id,
                Title = entity.Title,
                Category = entity.Category,
                Prompt = entity.Prompt,
                GeneratedText = entity.GeneratedText,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }
}
