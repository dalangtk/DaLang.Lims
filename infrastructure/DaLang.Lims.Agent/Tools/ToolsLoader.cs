using DaLang.Lims.Agent.Attibutes;
using DaLang.Lims.Agent.Options;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Reflection;

namespace DaLang.Lims.Agent.Tools;

public class ToolsLoader : IToolsLoader
{
    private readonly IServiceCollection _serviceCollection;
    private readonly ILogger<ToolsLoader> _logger;
    private readonly List<Type> _toolTypes = new();
    private readonly AgentOption _config;

    public ToolsLoader(IServiceCollection serviceCollection, ILogger<ToolsLoader> logger, AgentOption config)
    {
        _serviceCollection = serviceCollection;
        _logger = logger;
        _config = config;

        ScanTools();
    }

    /// <summary>
    /// 扫描程序集中的所有工具类型
    /// </summary>
    private void ScanTools()
    {
        List<Assembly> assembliesToScan = new List<Assembly>();
        if (_config.ScanAssemblies != null && _config.ScanAssemblies.Any())
        {
            foreach (var assemblyName in _config.ScanAssemblies)
            {
                try
                {
                    var assembly = Assembly.Load(assemblyName);
                    if (assembly != null)
                    {
                        assembliesToScan.Add(assembly);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }
        else
        {
            assembliesToScan = AppDomain.CurrentDomain.GetAssemblies().ToList();
        }
        foreach (var assembly in assembliesToScan)
        {
            try
            {
                var types = assembly.GetTypes()
                    .Where(t => t.IsClass && !t.IsAbstract && t.IsPublic)
                    .Where(t => IsToolType(t));

                foreach (var type in types)
                {
                    _toolTypes.Add(type);
                    _serviceCollection.AddScoped(type);
                    _logger.LogInformation("发现工具: {ToolName}", type.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "扫描程序集 {Assembly} 时出错", assembly.FullName);
            }
        }
    }

    /// <summary>
    /// 判断类型
    /// </summary>
    private bool IsToolType(Type type)
    {
        if (typeof(ITools).IsAssignableFrom(type))
        {
            return true;
        }
        if (type.GetCustomAttribute<AgentToolAttribute>() != null)
        {
            return true;
        }

        //检查是否有 [KernelFunction] 特性（兼容 Semantic Kernel）
        //var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
        //foreach (var method in methods)
        //{
        //    var kernelFuncAttr = method.GetCustomAttribute<ToolAttribute>();
        //    if (kernelFuncAttr != null)
        //    {
        //        return true;
        //    }
        //}

        return false;
    }

    /// <summary>
    /// 加载所有工具
    /// </summary>
    public List<AIFunction> LoadTools()
    {
        var tools = new List<AIFunction>();

        foreach (var toolType in _toolTypes)
        {
            try
            {
                var toolInstance = _serviceCollection.BuildServiceProvider().GetRequiredService(toolType);

                var methods = toolType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                    .Where(m => m.IsPublic && !m.IsSpecialName)
                    .Where(m => m.GetCustomAttribute<DescriptionAttribute>() != null ||
                                m.GetCustomAttribute<AgentToolAttribute>() != null);

                foreach (var method in methods)
                {
                    var aiFunction = AIFunctionFactory.Create(
                        method,
                        toolInstance,
                        new AIFunctionFactoryOptions
                        {
                            Name = method.Name,
                            Description = method.GetCustomAttribute<DescriptionAttribute>()?.Description
                                        ?? method.GetCustomAttribute<AgentToolAttribute>()?.Description
                                        ?? $"{method.Name} 方法"
                        }
                    );

                    tools.Add(aiFunction);
                    _logger.LogInformation("加载工具方法: {ClassName}.{MethodName}",
                        toolType.Name, method.Name);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "加载工具 {ToolType} 失败", toolType.Name);
            }
        }

        _logger.LogInformation("共加载 {Count} 个工具方法", tools.Count);
        return tools;
    }

    public List<Type> GetToolTypes() => _toolTypes;

    public Type? GetToolType(string name)
    {
        return _toolTypes.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
    }
}
