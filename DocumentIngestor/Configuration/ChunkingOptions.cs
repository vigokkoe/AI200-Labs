namespace DocumentIngestor.Configuration;

public sealed class ChunkingOptions
{
    public const string SectionName = "Chunking";

    public int MaxTokens { get; init; }

    public int OverlapTokens { get; init; }
}

public sealed class StorageOptions
{
    public const string SectionName = "Storage";

    public string AccountName { get; init; } = "";

    public string ContainerName { get; init; } = "";

    public string ApiKey { get; init; } = "";
}

public sealed class OutputOptions
{
    public const string SectionName = "Output";

    public string Directory { get; init; } = "output";
}