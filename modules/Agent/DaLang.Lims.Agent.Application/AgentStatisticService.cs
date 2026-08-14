using DaLang.Lims.Agent.Contracts;
using DaLang.Lims.Agent.Contracts.Dto;
using DaLang.Lims.Agent.Domain;
using DaLang.Lims.Agent.Services;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DaLang.Lims.Agent.Application;

[DynamicApi(Area = "agent")]
public class AgentStatisticService : BaseService, IAgentStatisticService, IDynamicApi
{
    private ISampleStatisticRepository _rep;
    public readonly IAgentService _agentService;
    private readonly ILogger<AgentStatisticService> _logger;
    public AgentStatisticService(IAgentService agentService, ILogger<AgentStatisticService> logger)
    {
        _agentService = agentService;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost]
    [NonFormatResult]
    public async Task<string> ExecuteQuery(UserInput input)
    {
        UserQueryResponse ret = new();
        var response = await _agentService.ProcessUserQueryAsync(input.Query);
        _logger.LogInformation("User query processed: {Query}", input.Query);
        ret.code = 0;
        ret.result = new Result
        {
            answer = response
        };

        var str = System.Text.Json.JsonSerializer.Serialize(ret);
        return str;
    }
}
