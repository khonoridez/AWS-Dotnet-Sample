using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Contracts;
using Worker.Application;

namespace Worker.Infrastructure;

public sealed class SqsConsumerBackgroundService(
    IAmazonSQS sqs,
    IImageJobProcessor processor,
    IOptions<SqsWorkerOptions> options,
    ILogger<SqsConsumerBackgroundService> logger) : BackgroundService
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web);
    private readonly SqsWorkerOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var receiveResponse = await sqs.ReceiveMessageAsync(new ReceiveMessageRequest
            {
                QueueUrl = _options.QueueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 20
            }, stoppingToken);

            foreach (var message in receiveResponse.Messages)
            {
                try
                {
                    var payload = JsonSerializer.Deserialize<ImageRequestedV1>(message.Body, JsonOptions);

                    if (payload is null)
                    {
                        logger.LogWarning("Skipping invalid message body.");
                        continue;
                    }

                    await processor.ProcessAsync(payload, stoppingToken);

                    await sqs.DeleteMessageAsync(_options.QueueUrl, message.ReceiptHandle, stoppingToken);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to process message. It will be retried by SQS.");
                }
            }
        }
    }
}