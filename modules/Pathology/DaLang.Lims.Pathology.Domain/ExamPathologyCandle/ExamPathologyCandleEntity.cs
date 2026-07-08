using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.ExamPathologyCandle;

/// <summary>
/// 蜡块 实体类
/// </summary>
/// <remarks>蜡块</remarks>
[SugarTable(TableName = "exam_pathology_candle")]
public partial class ExamPathologyCandleEntity : EntityTenant
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamInfoId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 蜡块号
    /// </summary>
    /// <remarks>蜡块号</remarks>
    [SugarColumn(ColumnName = "CandleNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int? CandleNo { get; set; }
    /// <summary>
    /// 原蜡块号
    /// </summary>
    /// <remarks>原蜡块号</remarks>
    [SugarColumn(ColumnName = "OriginalCandleNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? OriginalCandleNo { get; set; }
    /// <summary>
    /// 部位
    /// </summary>
    /// <remarks>部位</remarks>
    [SugarColumn(ColumnName = "Position", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? Position { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    /// <remarks>数量</remarks>
    [SugarColumn(ColumnName = "Amount", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int? Amount { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    /// <remarks>操作类型</remarks>
    [SugarColumn(ColumnName = "OperationType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int? OperationType { get; set; }
    /// <summary>
    /// 取材时间
    /// </summary>
    /// <remarks>取材时间</remarks>
    [SugarColumn(ColumnName = "EstimatedSamplingDate", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EstimatedSamplingDate { get; set; }
}

#pragma warning restore CS8618

