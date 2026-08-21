using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace SearchBootstrapper.Models;

public sealed class DocumentSearchModel
{
    [SimpleField(IsKey = true)]
    public string Id { get; init; } = string.Empty;

    [SearchableField(IsFilterable = true)]
    public string Title { get; init; } = string.Empty;

    [SearchableField]
    public string Content { get; init; } = string.Empty;

    [SimpleField(IsFilterable = true)]
    public string Source { get; init; } = string.Empty;
}