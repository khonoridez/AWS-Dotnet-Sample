using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;
using Services.Messaging;

namespace Api.Infrastructure.Aws.Sqs;

public sealed class SqsQueuePublisher : IQueuePublisher
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly IAmazonSQS _sqs;
    private readonly SqsOptions _options;

    public SqsQueuePublisher(IAmazonSQS sqs, IOptions<SqsOptions> options)
    {
        _sqs = sqs;
        _options = options.Value;
    }

    public Task PublishAsync<T>(T message, CancellationToken cancellationToken)
    {
        var body = JsonSerializer.Serialize(message, JsonOptions);

        return _sqs.SendMessageAsync(new SendMessageRequest
        {
            QueueUrl = _options.QueueUrl,
            MessageBody = body
        }, cancellationToken);
    }
}