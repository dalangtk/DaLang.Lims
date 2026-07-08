using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.ExamPathologyDigitalSlicing;

/// <summary>
/// 数字切片 实体类
/// </summary>
/// <remarks>数字切片</remarks>
[SugarTable(TableName = "exam_pathology_digital_slicing")]
public partial class ExamPathologyDigitalSlicingEntity : EntityBase
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamInfoId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long ExamInfoId { get; set; }
    /// <summary>
    /// 条码号
    /// </summary>
    /// <remarks>条码号</remarks>
    [SugarColumn(ColumnName = "Barcode", ColumnDataType = "varchar", Length = 32)]
    public string? Barcode { get; set; }
    /// <summary>
    /// 病理号
    /// </summary>
    /// <remarks>病理号</remarks>
    [SugarColumn(ColumnName = "SampleNo", ColumnDataType = "varchar", Length = 32)]
    public string? SampleNo { get; set; }
    /// <summary>
    /// 切片名称
    /// </summary>
    /// <remarks>切片名称</remarks>
    [SugarColumn(ColumnName = "SlicingName", ColumnDataType = "varchar", Length = 32)]
    public string? SlicingName { get; set; }
    /// <summary>
    /// 切片路径
    /// </summary>
    /// <remarks>切片路径</remarks>
    [SugarColumn(ColumnName = "SlicingPath", ColumnDataType = "varchar", Length = 256)]
    public string? SlicingPath { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

