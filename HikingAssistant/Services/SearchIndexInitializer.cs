using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Shared.Configuration;
using HikingAssistant.Models;
using Microsoft.Extensions.Options;

namespace HikingAssistant.Services;

class SearchIndexInitializer
{
  private readonly SearchIndexClient _searchIndexClient;
  private readonly string _indexName;

  public SearchIndexInitializer(SearchIndexClient searchIndexClient,
   IOptions<AzureSearchOptions> options)
  {
    _searchIndexClient = searchIndexClient;
    _indexName = options.Value.IndexName;
  }

  public async Task InitializeSearchIndexAsync()
  {
    var indexExists = await _searchIndexClient.GetIndexNamesAsync()
    .AnyAsync(name => name == _indexName);

    if (!indexExists)
    {
      var searchIndex = new SearchIndex(_indexName)
      {
        Fields = new FieldBuilder().Build(typeof(SearchDocument))
      };

      await _searchIndexClient.CreateOrUpdateIndexAsync(searchIndex);
    }
  }
}