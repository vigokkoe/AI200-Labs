using Azure.Search.Documents.Indexes.Models;

namespace SearchBootstrapper.Provisioning;

public interface ISearchIndexBuilder
{
    SearchIndex Build();
}