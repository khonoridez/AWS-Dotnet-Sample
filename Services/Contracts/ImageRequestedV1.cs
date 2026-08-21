namespace Services.Contracts;

public sealed record ImageRequestedV1(
    Guid RequestId,
    string ImageKey,
    string SourceUrl,
    DateTimeOffset RequestedAtUtc,
    string CorrelationId);