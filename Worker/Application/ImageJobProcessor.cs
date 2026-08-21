using Shared.Contracts;

namespace Worker.Application;

public sealed class ImageJobProcessor(ILogger<ImageJobProcessor> logger) : IImageJobProcessor
{
    public Task ProcessAsync(ImageRequestedV1 message, CancellationToken cancellationToken)
    {
        logger.LogInformation(
            "Processed message {RequestId} for image {ImageKey} ({SourceUrl})",
            message.RequestId,
            message.ImageKey,
            message.SourceUrl);

        return Task.CompletedTask;
    }
}