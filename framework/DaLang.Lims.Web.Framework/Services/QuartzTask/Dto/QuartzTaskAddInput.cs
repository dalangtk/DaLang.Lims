
using DaLang.Lims.Web.Framework.Core.QuartzTask.Enums;
using System;

namespace DaLang.Lims.Web.Framework.Services.QuartzTask.Dto;

/// <summary>
/// 定时任务新增输入
/// </summary>
public class QuartzTaskAddInput
{
    /// <summary>
    /// 任务名
    /// </summary>
    public string? TaskName { get; set; }
    /// <summary>
    /// 分组名
    /// </summary>
    public string? GroupName { get; set; }
    /// <summary>
    /// 触发器类型
    /// </summary>
    public TriggerTypeEnum TriggerType { get; set; }
    /// <summary>
    /// 间隔时间
    /// </summary>
    public string? Interval { get; set; }
    /// <summary>
    /// 任务描述
    /// </summary>
    public string? Describe { get; set; }
    /// <summary>
    /// 上次运行时间
    /// </summary>
    public DateTime? LastRunTime { get; set; }
    /// <summary>
    /// 上次运行时间
    /// </summary>
    public DateTime? NextRunTime { get; set; }
    /// <summary>
    /// 运行状态
    /// </summary>
    public int? Status { get; set; }
    /// <summary>
    /// 任务类型(1.DLL类型,2.API类型)
    /// </summary>
    public int? TaskType { get; set; }
    /// <summary>
    /// API参数
    /// </summary>
    public string? TaskParameter { get; set; }
    /// <summary>
    /// 调用的API地址
    /// </summary>
    public string? ApiUrl { get; set; }
    /// <summary>
    /// API访问类型
    /// </summary>
    public string? ApiRequestType { get; set; }
    /// <summary>
    /// API请求body
    /// </summary>
    public string? ApiRequestBody { get; set; }
    /// <summary>
    /// API请求头
    /// </summary>
    public string? ApiRequestHeader { get; set; }
    /// <summary>
    /// API超时时间
    /// </summary>
    public int? ApiTimeOut { get; set; }
    /// <summary>
    /// DLL名
    /// </summary>
    public string? DllName { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; } = true;
    /// <summary>
    /// 状态
    /// </summary>
    public string? StateDisplay { get; set; }
}