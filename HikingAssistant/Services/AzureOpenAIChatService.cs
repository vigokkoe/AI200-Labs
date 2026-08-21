using Azure;
using Azure.AI.OpenAI;
using HikingAssistant.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OpenAI.Chat;
using Shared.Configuration;

namespace HikingAssistant.Services;

public class AzureOpenAIChatService : IChatService
{
    private readonly ChatClient _chatClient;
    private readonly ChatCompletionOptions _options;
    private readonly ILogger<AzureOpenAIChatService> _logger;
    private readonly List<ChatMessage> _messages = [];
    
    public AzureOpenAIChatService(
      IOptions<AzureOpenAIOptions> options,
      ILogger<AzureOpenAIChatService> logger)
    {
        var client = new AzureOpenAIClient(
            new Uri(options.Value.Endpoint),
            new AzureKeyCredential(options.Value.ApiKey));

        _chatClient = client.GetChatClient(options.Value.Deployment);
        _options = new ChatCompletionOptions
        {
            Temperature = options.Value.Temperature,
            MaxOutputTokenCount = options.Value.MaxOutputTokens
        };
        _messages.Add(new SystemChatMessage(options.Value.SystemPrompt));
        _logger = logger;
    }

    public async Task<string> AskAsync(string question)
    {
      _logger.LogInformation($"Sending prompt to Azure OpenAI: {question}", question);

      _messages.Add(new UserChatMessage(GetUserPrompt(question)));
      var response = await _chatClient.CompleteChatAsync(
        // [
        //     new SystemChatMessage(options.Value.SystemPrompt),
        //     new UserChatMessage(question)
        // ],
        _messages,
        _options);

      _messages.Add(new AssistantChatMessage(response.Value.Content[0].Text));
      return response.Value.Content[0].Text;
    }

    private string GetUserPrompt(string question)
    {
      var document = File.ReadAllText("Knowledge/vacation.txt");

var prompt = $"""
Use ONLY the following document.

DOCUMENT

{document}

QUESTION

{question}

If the answer is not in the document,
say "I don't know."
""";
        return prompt;
    }
}