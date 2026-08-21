using Microsoft.AspNetCore.Mvc;
using Services.UseCases;

namespace Api.Controllers;

[ApiController]
[Route("api/image-jobs")]
public sealed class ImageJobsController : ControllerBase
{
    private readonly RequestImageService _requestImageService;

    public ImageJobsController(RequestImageService requestImageService)
    {
        _requestImageService = requestImageService;
    }

    public sealed record CreateImageJobRequest(string ImageKey, string SourceUrl);

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateImageJobRequest request, CancellationToken cancellationToken)
    {
        var correlationId = HttpContext.TraceIdentifier;

        await _requestImageService.EnqueueAsync(
            request.ImageKey,
            request.SourceUrl,
            correlationId,
            cancellationToken);

        return Accepted();
    }
}