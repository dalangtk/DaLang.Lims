
namespace DaLang.Lims.Pathology.Contracts.PathologyDisease.Dto;

/// <summary>
/// 疾病新增输入
/// </summary>
public partial class BasePathologyDiseaseAddInput
{
    /// <summary>
    /// 疾病代码
    /// </summary>
    public string? DiseaseCode { get; set; }
    /// <summary>
    /// 疾病名称
    /// </summary>
    public string? DiseaseName { get; set; }
    /// <summary>
    /// 是否恶性肿瘤
    /// </summary>
    public bool? IsMalignantTumor { get; set; }
    /// <summary>
    /// 是否非浸润性肿瘤
    /// </summary>
    public bool? IsNonInvasiveTumor { get; set; }
    /// <summary>
    /// 组织学分级
    /// </summary>
    public string? HistologicalLevel { get; set; }
    /// <summary>
    /// 免疫标记评估
    /// </summary>
    public string? LmmuneMarkerEvaluation { get; set; }
    /// <summary>
    /// 化疗反应
    /// </summary>
    public string? ChemotherapyReaction { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; } = true;
}