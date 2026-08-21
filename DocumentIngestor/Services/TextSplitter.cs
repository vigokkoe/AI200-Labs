using System.Text;
using System.Text.RegularExpressions;
using DocumentIngestor.Configuration;
using Shared.Models;
using Microsoft.Extensions.Options;


namespace DocumentIngestor.Services;

public sealed class TextSplitter : ITextSplitter
{
    private readonly ChunkingOptions _options;

    public TextSplitter(IOptions<ChunkingOptions> options)
    {
        _options = options.Value;
    }

    public IList<DocumentChunk> Split(string fileName, string text)
    {
        var maxChars = _options.MaxTokens * 4;
        var overlapChars = _options.OverlapTokens * 4;

        // Normalize line endings
        text = text.Replace("\r\n", "\n");

        // Split into sentences
        var sentences = Regex.Split(
            text,
            @"(?<=[.!?])\s+",
            RegexOptions.Multiline);

        var chunks = new List<DocumentChunk>();

        var builder = new StringBuilder();
        int chunkNumber = 1;

        foreach (var sentence in sentences)
        {
            if (string.IsNullOrWhiteSpace(sentence))
                continue;

            if (builder.Length > 0 &&
                builder.Length + sentence.Length > maxChars)
            {
                chunks.Add(CreateChunk(builder.ToString()));

                // keep overlap
                var overlap = builder.ToString();

                if (overlap.Length > overlapChars)
                    overlap = overlap[^overlapChars..];

                builder.Clear();
                builder.Append(overlap);
            }

            builder.Append(sentence);
            builder.Append(' ');
        }

        if (builder.Length > 0)
            chunks.Add(CreateChunk(builder.ToString()));

        return chunks;

        DocumentChunk CreateChunk(string content)
        {
            return new DocumentChunk
            {
                Id = Guid.NewGuid().ToString(),
                FileName = fileName,
                Title = Path.GetFileNameWithoutExtension(fileName),
                Content = content.Trim(),
                ChunkNumber = chunkNumber++,
                Source = fileName
            };
        }
    }
}

public interface ITextSplitter
{
    IList<DocumentChunk> Split(
        string fileName,
        string text);
}
