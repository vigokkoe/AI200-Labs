namespace Shared.Models;

public sealed class DocumentChunk
{
    public required string Id { get; init; }
    public required string FileName { get; init; }
    public required string Title { get; init; }
    public required string Content { get; init; }
    public int ChunkNumber { get; init; }
    public required string Source { get; init; }
    public ReadOnlyMemory<float> ContentVector { get; set; }
}