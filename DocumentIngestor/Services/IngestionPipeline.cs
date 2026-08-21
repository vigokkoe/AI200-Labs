using Microsoft.Extensions.Logging;

namespace DocumentIngestor.Services;

public sealed class IngestionPipeline : IIngestionPipeline
{
    private readonly IBlobReader _blobReader;
    private readonly IPdfExtractor _extractor;
    private readonly ITextSplitter _splitter;
    private readonly IEmbeddingGenerator _generator;
    private readonly IChunkWriter _writer;
    private readonly ILogger<IngestionPipeline> _logger;

  public IngestionPipeline(
      IBlobReader blobReader,
      IPdfExtractor extractor,
      ITextSplitter splitter,
      IChunkWriter writer,
      IEmbeddingGenerator generator,
      ILogger<IngestionPipeline> logger)
  {
    _blobReader = blobReader;
    _extractor = extractor;
    _splitter = splitter;
    _generator = generator;
    _writer = writer;
    _logger = logger;
  }

  public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        await foreach (var blob in _blobReader.GetDocumentsAsync(cancellationToken))
        {
            _logger.LogInformation($"Processing {blob.Name}");

            var text = await _extractor.ExtractAsync(blob, cancellationToken);

            var chunks = _splitter.Split(blob.Name, text);

            await _generator.GenerateAsync(chunks, cancellationToken);

            await _writer.WriteAsync(chunks, cancellationToken);

            _logger.LogInformation(
                $"{blob.Name}: {chunks.Count} chunks created");
        }
    }
}

public interface IIngestionPipeline
{
    Task RunAsync(CancellationToken cancellationToken = default);
}