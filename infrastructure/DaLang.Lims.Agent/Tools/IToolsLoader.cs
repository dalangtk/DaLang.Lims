using Microsoft.Extensions.AI;

namespace DaLang.Lims.Agent.Tools;

public interface IToolsLoader
{
    /// <summary>
    /// 加载所有工具
    /// </summary>
    List<AIFunction> LoadTools();

    /// <summary>
    /// 获取所有工具类型
    /// </summary>
    List<Type> GetToolTypes();

    /// <summary>
    /// 根据名称获取工具类型
    /// </summary>
    Type? GetToolType(string name);
}
