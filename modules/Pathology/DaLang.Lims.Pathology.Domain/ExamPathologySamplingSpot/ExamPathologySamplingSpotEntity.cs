using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.ExamPathologySamplingSpot;

/// <summary>
/// 病理检测取材部位-标本类型 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "exam_pathology_sampling_spot")]
public partial class ExamPathologySamplingSpotEntity : EntityTenant
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamInfoId", ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 采样部位代码
    /// </summary>
    /// <remarks>采样部位代码</remarks>
    [SugarColumn(ColumnName = "SamplingSpotCode", ColumnDataType = "varchar", Length = 16)]
    public string? SamplingSpotCode { get; set; }
    /// <summary>
    /// 标本类型代码
    /// </summary>
    /// <remarks>标本类型代码</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", ColumnDataType = "varchar", Length = 16)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }
}

#pragma warning restore CS8618

