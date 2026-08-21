namespace HikingAssistant.Interfaces;

public interface IChatService
{
    Task<string> AskAsync(string prompt);
}