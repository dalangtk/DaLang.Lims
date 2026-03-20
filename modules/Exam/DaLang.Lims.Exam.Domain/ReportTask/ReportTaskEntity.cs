using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.Exam.Domain.ReportTask;

/// <summary>
/// 报告任务 实体类
/// </summary>
/// <remarks>报告任务</remarks>
[SugarTable(TableName = "report_task")]
public partial class ReportTaskEntity : EntityTenant
{
    /// <summary>
    /// 检验信息Id
    /// </summary>
    /// <remarks>检验信息Id</remarks>
    [SugarColumn(ColumnName = "ExamInfoId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 条码
    /// </summary>
    [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 任务类型
    /// </summary>
    /// <remarks>任务类型</remarks>
    [SugarColumn(ColumnName = "TaskType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? TaskType { get; set; }
    /// <summary>
    /// 任务优先级
    /// </summary>
    /// <remarks>任务优先级</remarks>
    [SugarColumn(ColumnName = "TaskPriority", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int? TaskPriority { get; set; }
    /// <summary>
    /// 处理状态
    /// </summary>
    /// <remarks>处理状态</remarks>
    [SugarColumn(ColumnName = "ProcessStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "0", DecimalDigits = 11)]
    public int ProcessStatus { get; set; } = 0;
    /// <summary>
    /// 合并检验信息Id
    /// </summary>
    /// <remarks>合并检验信息Id</remarks>
    [SugarColumn(ColumnName = "ExamInfoIds", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 255)]
    public string? ExamInfoIds { get; set; }
    /// <summary>
    /// 合并信息
    /// </summary>
    /// <remarks>合并信息</remarks>
    [SugarColumn(ColumnName = "MergeInfo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 1024)]
    public string? MergeInfo { get; set; }
}
