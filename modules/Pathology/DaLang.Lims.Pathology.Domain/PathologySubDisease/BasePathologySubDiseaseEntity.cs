using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.BasePathologySubDisease;

/// <summary>
/// 子疾病 实体类
/// </summary>
/// <remarks>子疾病</remarks>
[SugarTable(TableName = "base_pathology_sub_disease")]
public partial class BasePathologySubDiseaseEntity : EntityBase
{
    /// <summary>
    /// 子疾病代码
    /// </summary>
    /// <remarks>子疾病代码</remarks>
    [SugarColumn(ColumnName = "SubDiseaseCode", ColumnDataType = "varchar", Length = 8)]
    public string? SubDiseaseCode { get; set; }
    /// <summary>
    /// 子疾病名称
    /// </summary>
    /// <remarks>子疾病名称</remarks>
    [SugarColumn(ColumnName = "SubDiseaseName", ColumnDataType = "varchar", Length = 32)]
    public string? SubDiseaseName { get; set; }
    /// <summary>
    /// 描述外链
    /// </summary>
    /// <remarks>描述外链</remarks>
    [SugarColumn(ColumnName = "ExplainUrl", ColumnDataType = "varchar", Length = 255)]
    public string? ExplainUrl { get; set; }
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

