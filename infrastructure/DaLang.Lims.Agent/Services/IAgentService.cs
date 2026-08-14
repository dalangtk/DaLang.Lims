namespace DaLang.Lims.Agent.Services;

public interface IAgentService
{
    Task<string> ProcessUserQueryAsync(string userMessage);
}
