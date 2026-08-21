using OpenAI.Embeddings;
using Shared.Models;

namespace DocumentIngestor.Services;

public class AzureOpenAiEmbeddingGenerator : IEmbeddingGenerator
{
    private readonly EmbeddingClient _client;

    public AzureOpenAiEmbeddingGenerator(EmbeddingClient embeddingClient)
    {
        _client = embeddingClient;
    }

    public async Task GenerateAsync(
        IList<DocumentChunk> chunks,
        CancellationToken cancellationToken = default)
    {
        foreach (var chunk in chunks)
        {
            OpenAIEmbedding embedding =
                await _client.GenerateEmbeddingAsync(
                    chunk.Content,
                    cancellationToken: cancellationToken);

            chunk.ContentVector = embedding.ToFloats();
        }
    }
}

public interface IEmbeddingGenerator
{
    /// <summary>
    /// Generates embeddings for a list of document chunks using Azure OpenAI Embeddings API.
    /// </summary>
    /// <param name="chunks">use IList<DocumentChunk> because the implementation updates the existing objects in place</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task GenerateAsync(
        IList<DocumentChunk> chunks,
        CancellationToken cancellationToken = default);
}