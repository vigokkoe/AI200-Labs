using AI200Labs.Shared.Extensions;
using Azure;
using Azure.AI.OpenAI;
using Azure.Storage;
using Azure.Storage.Blobs;
using DotNetEnv;
using DocumentIngestor.Configuration;
using DocumentIngestor.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Configuration;

Env.TraversePath().Load();

var builder = Host.CreateApplicationBuilder(args);

builder.AddAppSettings();

builder.Services.Configure<StorageOptions>(
    builder.Configuration.GetSection(StorageOptions.SectionName));

builder.Services.Configure<ChunkingOptions>(
    builder.Configuration.GetSection(ChunkingOptions.SectionName));

builder.Services.Configure<AzureOpenAIOptions>(
    builder.Configuration.GetSection(AzureOpenAIOptions.SectionName));

builder.Services.Configure<OutputOptions>(
    builder.Configuration.GetSection(OutputOptions.SectionName));

builder.Services.AddSingleton(sp =>
{
    var storage = sp.GetRequiredService<IOptions<StorageOptions>>().Value;
    
    var serviceClient = new BlobServiceClient(
      new Uri($"https://{storage.AccountName}.blob.core.windows.net"),
      new StorageSharedKeyCredential(storage.AccountName, storage.ApiKey));

    return serviceClient.GetBlobContainerClient(storage.ContainerName);
});

builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureOpenAIOptions>>().Value;

    var azureClient = new AzureOpenAIClient(
        new Uri(options.Endpoint),
        new AzureKeyCredential(options.ApiKey));

    return azureClient.GetEmbeddingClient(options.EmbeddingDeployment);
});

builder.Services.AddSingleton<IBlobReader, BlobReader>();
builder.Services.AddSingleton<IPdfExtractor, PdfExtractor>();
builder.Services.AddSingleton<ITextSplitter, TextSplitter>();
builder.Services.AddSingleton<IEmbeddingGenerator, AzureOpenAiEmbeddingGenerator>();
builder.Services.AddSingleton<IChunkWriter, JsonChunkWriter>();

builder.Services.AddSingleton<IIngestionPipeline, IngestionPipeline>();

var host = builder.Build();

var pipeline =
    host.Services.GetRequiredService<IIngestionPipeline>();

await pipeline.RunAsync();