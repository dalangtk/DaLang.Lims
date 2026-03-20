using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.Exam.Domain.ExamUnAuditLog;

// <summary>
/// 反审核记录 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "exam_unaudit_log")]
public partial class ExamUnAuditLogEntity : EntityTenant
{
    /// <summary>
    /// 检验信息Id
    /// </summary>
    /// <remarks>检验信息Id</remarks>
    [SugarColumn(ColumnName = "ExamInfoId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 反审核类型
    /// </summary>
    /// <remarks>反审核类型</remarks>
    [SugarColumn(ColumnName = "UnAuditType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? UnAuditType { get; set; }
    /// <summary>
    /// 反审核原因代码
    /// </summary>
    /// <remarks>反审核原因代码</remarks>
    [SugarColumn(ColumnName = "ReasonCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ReasonCode { get; set; }
    /// <summary>
    /// 反审核原因
    /// </summary>
    /// <remarks>反审核原因</remarks>
    [SugarColumn(ColumnName = "ReasonContent", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 512)]
    public string? ReasonContent { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCodes", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? PurCodes { get; set; }
    /// <summary>
    /// 目的名称
    /// </summary>
    /// <remarks>目的名称</remarks>
    [SugarColumn(ColumnName = "PurNames", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? PurNames { get; set; }
}
