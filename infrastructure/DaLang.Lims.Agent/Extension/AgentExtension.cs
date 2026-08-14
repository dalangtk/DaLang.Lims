using DaLang.Lims.Agent.Options;
using DaLang.Lims.Agent.Services;
using DaLang.Lims.Agent.Tools;
using Microsoft.Agents.AI;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenAI;
using System.ClientModel;

namespace DaLang.Lims.Agent.Extension;

public static class AgentExtension
{
    public static IServiceCollection AddAgent(this IServiceCollection services, IConfiguration configuration)
    {
        var agentConfig = configuration.GetSection("AgentConfig").Get<AgentOption>() ?? new();
        services.AddSingleton(sp => agentConfig);

        var toolsLoader = new ToolsLoader(services, services.BuildServiceProvider().GetRequiredService<ILogger<ToolsLoader>>(), agentConfig);

        services.AddSingleton<IToolsLoader>(toolsLoader);
        services.AddScoped<IAgentService, AgentService>();

        var modelId = agentConfig.ModelId;
        if (string.IsNullOrWhiteSpace(modelId))
        {
            throw new Exception("no model id configured, please set AgentConfig:ModelId in appsettings.json");
        }
        var apiKey = agentConfig.ApiKey;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            throw new Exception("no api key configured, please set AgentConfig:ApiKey in appsettings.json");
        }
        var endpoint = agentConfig.Endpoint;
        if (string.IsNullOrWhiteSpace(endpoint))
        {
            throw new Exception("no endpoint configured, please set AgentConfig:Endpoint in appsettings.json");
        }

        var openAiClient = new OpenAIClient(new ApiKeyCredential(apiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(endpoint),
            }
        );

        var chatClient = openAiClient.GetChatClient(modelId);
        var chatClientWrapper = chatClient.AsIChatClient();
        var skillsProvider = new AgentSkillsProvider(skillPath: Path.Combine(AppContext.BaseDirectory, "skills"));

        //工具自动注册
        //var toolLoader = toolsLoader;
        var tools = toolsLoader.LoadTools();
        var agent = chatClientWrapper.AsAIAgent(new ChatClientAgentOptions
        {
            Name = "dalang-lims-agent",
            ChatOptions = new ChatOptions
            {
                Instructions = @$"你是一个数据查询助手，同时还是一个系统功能助手，专门查询数据和指导用户使用系统。
【核心规则】根据用户输入，选择调用工具。
禁止自己编造数据。
例如如果用户要查询某个时间段内客户发放报告数，你必须调用 GetReportCountGroupByCusomerAndTestate 工具来获取真实数据。
【参数提取规则】
- '上个月'：startTime={DateTime.Now.AddMonths(-1):yyyy-MM-01}, endTime={DateTime.Now.AddDays(-DateTime.Now.Day):yyyy-MM-dd}
- '最近7天'：startTime={DateTime.Now.AddDays(-7):yyyy-MM-dd}, endTime={DateTime.Now:yyyy-MM-dd}
- '本月'：startTime={DateTime.Now:yyyy-MM-01}, endTime={DateTime.Now:yyyy-MM-dd}
如果用户没有明确指定某些参数，请根据上下文推断最合理的值。
如果用户需要图表，请使用Echarts。
得到查询结果后，用自然、友好的语言回复用户。",
                Tools = tools.ToArray()
            },
            AIContextProviders = [skillsProvider]
        })
            .AsBuilder()
            .UseToolApproval(new ToolApprovalAgentOptions
            {
                AutoApprovalRules = [AgentSkillsProvider.AllToolsAutoApprovalRule],
            })
            .Build();

        services.AddSingleton<AIAgent>(agent);
        return services;
    }
    public static IApplicationBuilder UseAgent(this IApplicationBuilder builder)
    {
        return builder;
    }
}
