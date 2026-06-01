using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.BasePathologyDisease;

/// <summary>
/// 疾病实体类
/// </summary>
/// <remarks>疾病</remarks>
[SugarTable(TableName = "base_pathology_disease")]
public partial class BasePathologyDiseaseEntity : EntityBase
{
    /// <summary>
    /// 疾病代码
    /// </summary>
    /// <remarks>疾病代码</remarks>
    [SugarColumn(ColumnName = "DiseaseCode", ColumnDataType = "varchar", Length = 16)]
    public string? DiseaseCode { get; set; }
    /// <summary>
    /// 疾病名称
    /// </summary>
    /// <remarks>疾病名称</remarks>
    [SugarColumn(ColumnName = "DiseaseName", ColumnDataType = "varchar", Length = 64)]
    public string? DiseaseName { get; set; }
    /// <summary>
    /// 是否恶性肿瘤
    /// </summary>
    /// <remarks>是否恶性肿瘤</remarks>
    [SugarColumn(ColumnName = "IsMalignantTumor", ColumnDataType = "tinyint")]
    public bool? IsMalignantTumor { get; set; }
    /// <summary>
    /// 是否非浸润性肿瘤
    /// </summary>
    /// <remarks>是否非浸润性肿瘤</remarks>
    [SugarColumn(ColumnName = "IsNonInvasiveTumor", ColumnDataType = "tinyint")]
    public bool? IsNonInvasiveTumor { get; set; }
    /// <summary>
    /// 组织学分级
    /// </summary>
    /// <remarks>组织学分级</remarks>
    [SugarColumn(ColumnName = "HistologicalLevel", ColumnDataType = "varchar", Length = 16)]
    public string? HistologicalLevel { get; set; }
    /// <summary>
    /// 免疫标记评估
    /// </summary>
    /// <remarks>免疫标记评估</remarks>
    [SugarColumn(ColumnName = "LmmuneMarkerEvaluation", ColumnDataType = "varchar", Length = 16)]
    public string? LmmuneMarkerEvaluation { get; set; }
    /// <summary>
    /// 化疗反应
    /// </summary>
    /// <remarks>化疗反应</remarks>
    [SugarColumn(ColumnName = "ChemotherapyReaction", ColumnDataType = "varchar", Length = 16)]
    public string? ChemotherapyReaction { get; set; }
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

