
using System.Text.Json;
using AI200Labs.Shared.Extensions;
using Shared.Models;

namespace DocumentIngestor.Services;

public sealed class JsonChunkWriter : IChunkWriter
{
    public async Task WriteAsync(
        IEnumerable<DocumentChunk> chunks,
        CancellationToken cancellationToken = default)
    {
        Directory.CreateDirectory("output");

        foreach (var group in chunks.GroupBy(c => c.FileName))
        {
            var fileName = Path.Combine(
                "output",
                $"{group.Key.GetSafeFileNameWithoutExtension()}.json");

            await File.WriteAllTextAsync(
                fileName,
                JsonSerializer.Serialize(
                    group,
                    new JsonSerializerOptions
                    {
                        WriteIndented = true
                    }),
                cancellationToken);
        }
    }
}

public interface IChunkWriter
{
    Task WriteAsync(
        IEnumerable<DocumentChunk> chunks,
        CancellationToken cancellationToken = default);
}