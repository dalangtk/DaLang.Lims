using System;

namespace DaLang.Lims.Web.Framework.Contracts.SysQuartzTaskLog.Dto;

/// <summary>
/// 任务日志新增输入
/// </summary>
public partial class QuartzTaskLogAddInput
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public long? TaskId { get; set; }
    /// <summary>
    /// 任务开始时间
    /// </summary>
    /// <remarks>任务开始时间</remarks>
    public DateTime? BeginDate { get; set; }
    /// <summary>
    /// 任务结束时间
    /// </summary>
    /// <remarks>任务结束时间</remarks>
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 任务耗时(毫秒)
    /// </summary>
    /// <remarks>任务耗时(毫秒)</remarks>
    public int? DurationMs { get; set; }
    /// <summary>
    /// 任务执行结果
    /// </summary>
    /// <remarks>任务执行结果</remarks>
    public string? Msg { get; set; }
    /// <summary>
    /// 任务执行状态,0正常,1异常
    /// </summary>
    /// <remarks>任务执行状态,0正常,1异常</remarks>
    public int? JobStatus { get; set; }
}