using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;
using System;

#pragma warning disable CS8618
namespace DaLang.Lims.Web.Framework.Domain.SysQuartzTaskLog;

/// <summary>
/// 任务日志 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "sys_quartz_task_log")]
public class QuartzTaskLogEntity : EntityTenant
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "TaskId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? TaskId { get; set; }
    /// <summary>
    /// 任务开始时间
    /// </summary>
    /// <remarks>任务开始时间</remarks>
    [SugarColumn(ColumnName = "BeginDate", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? BeginDate { get; set; }
    /// <summary>
    /// 任务结束时间
    /// </summary>
    /// <remarks>任务结束时间</remarks>
    [SugarColumn(ColumnName = "EndDate", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 任务耗时(毫秒)
    /// </summary>
    /// <remarks>任务耗时(毫秒)</remarks>
    [SugarColumn(ColumnName = "DurationMs", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? DurationMs { get; set; }
    /// <summary>
    /// 任务执行结果
    /// </summary>
    /// <remarks>任务执行结果</remarks>
    [SugarColumn(ColumnName = "Msg", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 1024)]
    public string? Msg { get; set; }
    /// <summary>
    /// 任务执行状态,0正常,1异常
    /// </summary>
    /// <remarks>任务执行状态,0正常,1异常</remarks>
    [SugarColumn(ColumnName = "JobStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int? JobStatus { get; set; }
}

#pragma warning restore CS8618

