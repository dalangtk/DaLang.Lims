using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Shared.Domain.ExamSampleTrack;

/// <summary>
/// 样本跟踪 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "exam_sample_track")]
public partial class ExamSampleTrackEntity : EntityTenant
{
    /// <summary>
    /// 条码
    /// </summary>
    /// <remarks>条码</remarks>
    [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? GroupCode { get; set; }
    /// <summary>
    /// 组别名称
    /// </summary>
    /// <remarks>组别名称</remarks>
    [SugarColumn(ColumnName = "GroupName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? GroupName { get; set; }
    /// <summary>
    /// 检测日期
    /// </summary>
    /// <remarks>检测日期</remarks>
    [SugarColumn(ColumnName = "TestDate", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "date")]
    public DateTime? TestDate { get; set; }
    /// <summary>
    /// 流水号
    /// </summary>
    /// <remarks>流水号</remarks>
    [SugarColumn(ColumnName = "SampleNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? SampleNo { get; set; }
    /// <summary>
    /// 记录内容
    /// </summary>
    /// <remarks>记录内容</remarks>
    [SugarColumn(ColumnName = "TrackContent", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 512)]
    public string? TrackContent { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    /// <remarks>操作类型</remarks>
    [SugarColumn(ColumnName = "OperationType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? OperationType { get; set; }
}

#pragma warning restore CS8618

