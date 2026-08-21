namespace Api.Infrastructure.Aws.Sqs;

public sealed class SqsOptions
{
    public const string SectionName = "Sqs";
    public required string QueueUrl { get; init; }
}