using Microsoft.Agents.AI;
using Microsoft.Extensions.Logging;

namespace DaLang.Lims.Agent.Services;

public class AgentService : IAgentService
{
    private readonly AIAgent _agent;
    private readonly ILogger<AgentService> _logger;
    public AgentService(AIAgent agent, ILogger<AgentService> logger)
    {
        _agent = agent;
        _logger = logger;
    }
    public async Task<string> ProcessUserQueryAsync(string userMessage)
    {
        try
        {
            _logger.LogInformation("用户提问: {Message}", userMessage);
            var result = await _agent.RunAsync(userMessage);
            var response = result.Messages.LastOrDefault()?.Text ?? "抱歉，无法处理您的请求。";

            _logger.LogInformation("AI响应: {Response}", response);
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AI Agent处理失败");
            return $"抱歉，处理您的请求时出现错误: {ex.Message}";
        }
    }
}
