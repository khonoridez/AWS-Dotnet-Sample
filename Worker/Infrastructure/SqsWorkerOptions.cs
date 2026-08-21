namespace Worker.Infrastructure;

public sealed class SqsWorkerOptions
{
    public const string SectionName = "Sqs";

    public required string QueueUrl { get; init; }
}