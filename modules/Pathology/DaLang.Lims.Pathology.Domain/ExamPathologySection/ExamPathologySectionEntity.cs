using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.ExamPathologySection;

/// <summary>
/// 切片 实体类
/// </summary>
/// <remarks>切片</remarks>
[SugarTable(TableName = "exam_pathology_section")]
public partial class ExamPathologySectionEntity : EntityTenant
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamInfoId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 蜡块Id
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "CandleId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? CandleId { get; set; }
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
    /// 切片号
    /// </summary>
    /// <remarks>切片号</remarks>
    [SugarColumn(ColumnName = "SectionNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int? SectionNo { get; set; }
    /// <summary>
    /// 评估
    /// </summary>
    /// <remarks>评估</remarks>
    [SugarColumn(ColumnName = "Assessment", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? Assessment { get; set; }
    /// <summary>
    /// 反馈
    /// </summary>
    /// <remarks>反馈</remarks>
    [SugarColumn(ColumnName = "FeedBack", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? FeedBack { get; set; }
    /// <summary>
    /// 是否临时医嘱
    /// </summary>
    /// <remarks>是否临时医嘱</remarks>
    [SugarColumn(ColumnName = "IsMedicalAdvice", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool? IsMedicalAdvice { get; set; }
    /// <summary>
    /// 临时医嘱发起人
    /// </summary>
    /// <remarks>临时医嘱发起人</remarks>
    [SugarColumn(ColumnName = "MedicalAdviceUser", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? MedicalAdviceUser { get; set; }
}

#pragma warning restore CS8618

