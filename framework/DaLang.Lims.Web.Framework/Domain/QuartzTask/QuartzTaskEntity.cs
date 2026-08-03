using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Core.QuartzTask.Enums;
using SqlSugar;
using System;

#pragma warning disable CS8618
namespace DaLang.Lims.Web.Framework.Domain.SysQuartzTask;

/// <summary>
/// 定时任务 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "sys_quartz_task")]
public class QuartzTaskEntity : EntityTenant
{
    /// <summary>
    /// 任务名
    /// </summary>
    /// <remarks>任务名</remarks>
    [SugarColumn(ColumnName = "TaskName", ColumnDataType = "varchar", Length = 32)]
    public string? TaskName { get; set; }
    /// <summary>
    /// 分组名
    /// </summary>
    /// <remarks>分组名</remarks>
    [SugarColumn(ColumnName = "GroupName", ColumnDataType = "varchar", Length = 32)]
    public string? GroupName { get; set; }
    /// <summary>
    /// 触发器类型
    /// </summary>
    [SugarColumn(ColumnName = "TriggerType", ColumnDataType = "int", Length = 2)]
    public TriggerTypeEnum TriggerType { get; set; }
    /// <summary>
    /// 间隔时间
    /// </summary>
    /// <remarks>间隔时间</remarks>
    [SugarColumn(ColumnName = "Interval", ColumnDataType = "varchar", Length = 32)]
    public string? Interval { get; set; }
    /// <summary>
    /// 任务描述
    /// </summary>
    /// <remarks>任务描述</remarks>
    [SugarColumn(ColumnName = "Describe", ColumnDataType = "varchar", Length = 64)]
    public string? Describe { get; set; }
    /// <summary>
    /// 上次运行时间
    /// </summary>
    /// <remarks>上次运行时间</remarks>
    [SugarColumn(ColumnName = "LastRunTime", ColumnDataType = "datetime")]
    public DateTime? LastRunTime { get; set; }
    /// <summary>
    /// 运行状态
    /// </summary>
    /// <remarks>运行状态</remarks>
    [SugarColumn(ColumnName = "Status", ColumnDataType = "int", DecimalDigits = 2)]
    public int? Status { get; set; }
    /// <summary>
    /// 任务类型(1.DLL类型,2.API类型)
    /// </summary>
    /// <remarks>任务类型(1.DLL类型,2.API类型)</remarks>
    [SugarColumn(ColumnName = "TaskType", ColumnDataType = "int", DecimalDigits = 2)]
    public int? TaskType { get; set; }
    /// <summary>
    /// 作业参数
    /// </summary>
    /// <remarks>作业参数</remarks>
    [SugarColumn(ColumnName = "TaskParameter", ColumnDataType = "varchar", Length = 256)]
    public string? TaskParameter { get; set; }
    /// <summary>
    /// 调用的API地址
    /// </summary>
    /// <remarks>调用的API地址</remarks>
    [SugarColumn(ColumnName = "ApiUrl", ColumnDataType = "varchar", Length = 128)]
    public string? ApiUrl { get; set; }
    /// <summary>
    /// API访问类型
    /// </summary>
    /// <remarks>API访问类型</remarks>
    [SugarColumn(ColumnName = "ApiRequestType", ColumnDataType = "varchar", Length = 32)]
    public string? ApiRequestType { get; set; }
    /// <summary>
    /// API请求body
    /// </summary>
    /// <remarks>API请求body</remarks>
    [SugarColumn(ColumnName = "ApiRequestBody", ColumnDataType = "varchar", Length = 256)]
    public string? ApiRequestBody { get; set; }
    /// <summary>
    /// API请求头
    /// </summary>
    /// <remarks>API请求头</remarks>
    [SugarColumn(ColumnName = "ApiRequestHeader", ColumnDataType = "varchar", Length = 256)]
    public string? ApiRequestHeader { get; set; }
    /// <summary>
    /// API超时时间
    /// </summary>
    /// <remarks>API超时时间</remarks>
    [SugarColumn(ColumnName = "ApiTimeOut", ColumnDataType = "int", DecimalDigits = 11)]
    public int? ApiTimeOut { get; set; }
    /// <summary>
    /// DLL名
    /// </summary>
    /// <remarks>DLL名</remarks>
    [SugarColumn(ColumnName = "DllName", ColumnDataType = "varchar", Length = 64)]
    public string? DllName { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

