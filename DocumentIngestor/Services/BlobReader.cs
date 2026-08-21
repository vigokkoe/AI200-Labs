
using Azure.Storage.Blobs;
using System.Runtime.CompilerServices;

namespace DocumentIngestor.Services;

public sealed class BlobReader : IBlobReader
{
    private readonly BlobContainerClient _container;

    public BlobReader(BlobContainerClient container)
    {
        _container = container;
    }

    public async IAsyncEnumerable<BlobClient> GetDocumentsAsync(
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        await foreach (var blob in _container.GetBlobsAsync(
                           cancellationToken: cancellationToken))
        {
            yield return _container.GetBlobClient(blob.Name);
        }
    }
}

public interface IBlobReader
{
    IAsyncEnumerable<BlobClient> GetDocumentsAsync(
      CancellationToken cancellationToken = default);
}