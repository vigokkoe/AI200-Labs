namespace Shared.Configuration;

public sealed class AzureOpenAIOptions
{
  public const string SectionName = "AzureOpenAI";

  public string Endpoint { get; init; } = "";
  public string Deployment { get; init; } = "";
  public string ApiKey { get; init; } = "";
  public string EmbeddingDeployment { get; set; } = "";
  // Temperature controls randomness 0 - 1, lower is more deterministic, higher is more random
  public float Temperature { get; init; } = 0.2f;
  public int MaxOutputTokens { get; init; } = 500;
  public string SystemPrompt { get; init; } = "";
}