using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;

namespace SearchBootstrapper.Models;

public sealed class DocumentChunk
{
    [SimpleField(IsKey = true)]
    public string Id { get; init; } = "";

    [SearchableField(
        IsFilterable = true,
        IsSortable = true)]
    public string FileName { get; init; } = "";

    [SearchableField]
    public string Title { get; init; } = "";

    [SearchableField]
    public string Content { get; init; } = "";

    [SimpleField(
        IsFilterable = true,
        IsSortable = true)]
    public int ChunkNumber { get; init; }

    [SimpleField(
        IsFilterable = true)]
    public string Source { get; init; } = "";
   
    // we'll populate this next
    public ReadOnlyMemory<float> ContentVector { get; set; }
}