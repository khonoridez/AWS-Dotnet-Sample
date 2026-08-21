namespace Services.Messaging;

public interface IQueuePublisher
{
    Task PublishAsync<T>(T message, CancellationToken cancellationToken);
}