using Shared.Configuration;
using HikingAssistant.Interfaces;
using HikingAssistant.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AI200Labs.Shared.Extensions;

// Load .env into environment variables
DotNetEnv.Env.TraversePath().Load();

var builder = Host.CreateApplicationBuilder(args);

builder.AddAppSettings();

// Configuration
builder.Services.Configure<AzureOpenAIOptions>(
    builder.Configuration.GetSection(AzureOpenAIOptions.SectionName));

// Logging
builder.Services.AddLogging();

// Services
builder.Services.AddSingleton<IChatService, AzureOpenAIChatService>();

var host = builder.Build();

var chatService = host.Services.GetRequiredService<IChatService>();

Console.WriteLine("=== Hiking Assistant ===");
Console.WriteLine();

while (true)
{
  Console.Write("> ");

  var question = Console.ReadLine();

  if (string.IsNullOrWhiteSpace(question))
    break;

  Console.WriteLine();

  try
  {
    var answer = await chatService.AskAsync(question);

    Console.WriteLine(answer);
  }
  catch (Exception ex)
  {
    Console.WriteLine($"Error: {ex.Message}");
  }

  Console.WriteLine();
}