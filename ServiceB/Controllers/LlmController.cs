using LLMProxy.Clients;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace LlmProxyApi.Controllers;

[ApiController]
[Route("api/llm")]
[Authorize]
public class LlmController : ControllerBase
{
    private readonly OpenAiClient _openAiClient;

    public LlmController(OpenAiClient openAiClient)
    {
        _openAiClient = openAiClient;
    }

    /// <summary>
    /// Tar emot en prompt och returnerar genererad text från OpenAI.
    /// </summary>
    [HttpPost("generate")]
    [ProducesResponseType(typeof(GenerateTextResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<GenerateTextResponse>> Generate(
        [FromBody] GenerateTextRequest request,
        CancellationToken ct)
    {
        var generatedText = await _openAiClient.GenerateAsync(
            request.Prompt,
            ct);
        return Ok(new GenerateTextResponse
        {
            GeneratedText = generatedText
        });
    }
}

public class GenerateTextRequest
{
    [Required]
    [StringLength(4000)]
    public string Prompt { get; set; } = string.Empty;
}

public class GenerateTextResponse
{
    public string GeneratedText { get; set; } = string.Empty;
}