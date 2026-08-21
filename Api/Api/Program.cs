using Amazon.SQS;
using Api.Infrastructure.Aws.Sqs;
using Services.Messaging;
using Services.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAWSService<IAmazonSQS>();
builder.Services.Configure<SqsOptions>(builder.Configuration.GetSection(SqsOptions.SectionName));

builder.Services.AddScoped<IQueuePublisher, SqsQueuePublisher>();
builder.Services.AddScoped<RequestImageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
