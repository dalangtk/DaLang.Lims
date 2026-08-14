using DaLang.Lims.Agent.Contracts.Dto;

namespace DaLang.Lims.Agent.Contracts;

public interface IAgentStatisticService
{
    Task<string> ExecuteQuery(UserInput input);
}
