using Services.Contracts;
using Services.Messaging;

namespace Services.UseCases;

public sealed class RequestImageService
{
    private readonly IQueuePublisher _publisher;

    public RequestImageService(IQueuePublisher publisher)
    {
        _publisher = publisher;
    }

    public Task EnqueueAsync(string imageKey, string sourceUrl, string correlationId, CancellationToken cancellationToken)
    {
        var message = new ImageRequestedV1(
            Guid.NewGuid(),
            imageKey,
            sourceUrl,
            DateTimeOffset.UtcNow,
            correlationId);

        return _publisher.PublishAsync(message, cancellationToken);
    }
}