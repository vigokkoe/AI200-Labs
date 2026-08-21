
using System.Text;
using Azure.Storage.Blobs;
using UglyToad.PdfPig;

namespace DocumentIngestor.Services;

public sealed class PdfExtractor : IPdfExtractor
{
    public async Task<string> ExtractAsync(
        BlobClient blob,
        CancellationToken cancellationToken = default)
    {
        var download = await blob.DownloadStreamingAsync(
            cancellationToken: cancellationToken);

        await using var memory = new MemoryStream();

        await download.Value.Content.CopyToAsync(memory, cancellationToken);

        memory.Position = 0;

        using var pdf = PdfDocument.Open(memory);

        var builder = new StringBuilder();

        foreach (var page in pdf.GetPages())
        {
            builder.AppendLine(page.Text);
        }

        return builder.ToString();
    }
}

public interface IPdfExtractor
{
    Task<string> ExtractAsync(
        BlobClient blob,
        CancellationToken cancellationToken = default);
}