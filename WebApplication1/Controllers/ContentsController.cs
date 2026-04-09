using ContentAPI.DTOs;
using ContentAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ContentAPI.Controllers
{
    [ApiController]
    [Route("api/contents")]
    public class ContentsController : ControllerBase
    {
        private readonly IAiContentService _service;

        public ContentsController(IAiContentService service)
        {
            _service = service;
        }

        /// <summary>
        /// Hämtar allt sparat AI-innehåll med valfri filtrering och sortering.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AiContentResponse>>> GetAll(
            [FromQuery] string? category,
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string? sort = "-createdAt")
        {
            var items = await _service.GetAllAsync(category, startDate, endDate, sort);
            return Ok(items);
        }

        /// <summary>
        /// Hämtar ett specifikt innehåll via id.
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<AiContentResponse>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>
        /// Skapar nytt AI-innehåll.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<AiContentResponse>> Create([FromBody] CreateAiContentRequest request)
        {
            var created = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        /// <summary>
        /// Uppdaterar befintligt AI-innehåll.
        /// </summary>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAiContentRequest request)
        {
            var updated = await _service.UpdateAsync(id, request);
            return updated ? NoContent() : NotFound();
        }

        /// <summary>
        /// Tar bort innehåll via id.
        /// </summary>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        /// <summary>
        /// Genererar text för ett sparat innehåll via LLM Proxy API.
        /// </summary>
        [HttpPost("{id:int}/generate")]
        public async Task<ActionResult<AiContentResponse>> Generate(int id, CancellationToken ct)
        {
            var result = await _service.GenerateAsync(id, ct);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
