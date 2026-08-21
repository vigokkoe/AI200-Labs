using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Shared.Configuration;

namespace SearchBootstrapper.Provisioning;

public sealed class SearchProvisioner : ISearchProvisioner
{
    private readonly SearchIndexClient _indexClient;
    private readonly ISearchIndexBuilder _indexBuilder;
    private readonly string _indexName;
    private readonly ILogger<SearchProvisioner> _logger;

  public SearchProvisioner(
        SearchIndexClient indexClient,
        ISearchIndexBuilder indexBuilder,
        IOptions<AzureSearchOptions> options,
        ILogger<SearchProvisioner> logger)
    {
        _indexClient = indexClient;
        _indexBuilder = indexBuilder;
        _indexName = options.Value.IndexName;
        _logger = logger;
    }

    public async Task ProvisionAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Checking Azure AI Search...");

        var indexes = new List<string>();

        await foreach (var index in _indexClient.GetIndexesAsync(cancellationToken))
        {
            indexes.Add(index.Name);
            PrintIndexSchema(index);
        }

        if (indexes.Contains(_indexName))
        {
            _logger.LogInformation("Index '{Index}' already exists.", _indexName);
            return;
        }

        _logger.LogInformation("Creating index '{Index}'...", _indexName);

        await CreateDocumentIndexAsync(cancellationToken);

        _logger.LogInformation("Done.");
    }

    private async Task CreateDocumentIndexAsync(CancellationToken cancellationToken)
    {
        var index = _indexBuilder.Build();

        await _indexClient.CreateIndexAsync(
            index,
            cancellationToken);
        PrintIndexSchema(index);
    }

    private static void PrintIndexSchema(SearchIndex index)
    {
        Console.WriteLine($"Index: {index.Name}");

        foreach (var field in index.Fields)
        {
            Console.WriteLine(
                $"{field.Name,-20} {field.Type,-15} " +
                $"Searchable={field.IsSearchable}");
        }
    }
}