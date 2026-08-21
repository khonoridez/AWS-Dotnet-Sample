using Shared.Contracts;

namespace Worker.Application;

public interface IImageJobProcessor
{
    Task ProcessAsync(ImageRequestedV1 message, CancellationToken cancellationToken);
}