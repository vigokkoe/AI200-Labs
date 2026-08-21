using Azure;
using Azure.Search.Documents.Indexes;
using DotNetEnv;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using SearchBootstrapper.Provisioning;
using AI200Labs.Shared.Extensions;

Env.TraversePath().Load();

var builder = Host.CreateApplicationBuilder(args);

builder.AddAppSettings();

// read configuration from appsettings.json
builder.Services.Configure<AzureSearchOptions>(
    builder.Configuration.GetSection(AzureSearchOptions.SectionName));

// create SearchIndexClient using AzureSearchOptions
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<AzureSearchOptions>>().Value;

    return new SearchIndexClient(
        new Uri(options.Endpoint),
        new AzureKeyCredential(options.ApiKey));
});

builder.Services.AddSingleton<ISearchIndexBuilder, DocumentChunkIndexBuilder>();

builder.Services.AddSingleton<ISearchProvisioner, SearchProvisioner>();

var host = builder.Build();

var provisioner = host.Services.GetRequiredService<ISearchProvisioner>();

await provisioner.ProvisionAsync();