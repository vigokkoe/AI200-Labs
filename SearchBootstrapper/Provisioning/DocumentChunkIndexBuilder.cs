using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using Microsoft.Extensions.Options;
using Shared.Configuration;
using SearchBootstrapper.Models;

namespace SearchBootstrapper.Provisioning;

public sealed class DocumentChunkIndexBuilder
    : ISearchIndexBuilder
{
    private readonly AzureSearchOptions _options;

    public DocumentChunkIndexBuilder(
        IOptions<AzureSearchOptions> options)
    {
        _options = options.Value;
    }

    public SearchIndex Build()
    {
        var fieldBuilder = new FieldBuilder();

        // return new SearchIndex(_options.IndexName)
        // {
        //     Fields = fieldBuilder.Build(typeof(DocumentChunk))
        // };
        var vectorSearch = new VectorSearch
        {
            Profiles =
            {
                new VectorSearchProfile(
                    "vector-profile",
                    "hnsw")
            },
            Algorithms =
            {
                new HnswAlgorithmConfiguration("hnsw")
            }
        };
        var index = new SearchIndex(_options.IndexName)
        {
            Fields = fieldBuilder.Build(typeof(DocumentChunk)),
            VectorSearch = vectorSearch
        };

    return index;
    }
}