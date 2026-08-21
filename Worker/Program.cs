using Amazon.SQS;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Worker.Application;
using Worker.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.Configure<SqsWorkerOptions>(builder.Configuration.GetSection(SqsWorkerOptions.SectionName));

builder.Services.AddSingleton<IImageJobProcessor, ImageJobProcessor>();
builder.Services.AddHostedService<SqsConsumerBackgroundService>();

await builder.Build().RunAsync();