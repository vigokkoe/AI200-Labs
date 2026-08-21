namespace Shared.Configuration;

public sealed class AzureSearchOptions
{
    public const string SectionName = "AzureSearch";

    public string Endpoint { get; init; } = "";

    public string IndexName { get; init; } = "";

    public string ApiKey { get; init; } = "";
}