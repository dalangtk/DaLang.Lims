namespace DaLang.Lims.Agent.Tools;

public interface ITools
{
    string ToolName { get; }
    string Description { get; }
}
public interface IAgentTool<TRequest, TResponse> : ITools
{
    Task<TResponse> ExecuteAsync(TRequest request);
}
