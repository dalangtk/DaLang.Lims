
namespace DaLang.Lims.Pathology.Contracts.BasePathologySubDisease.Dto;

/// <summary>
/// 子疾病新增输入
/// </summary>
public partial class BasePathologySubDiseaseAddInput
{
    /// <summary>
    /// 子疾病代码
    /// </summary>
    public string? SubDiseaseCode { get; set; }
    /// <summary>
    /// 子疾病名称
    /// </summary>
    public string? SubDiseaseName { get; set; }
    /// <summary>
    /// 描述外链
    /// </summary>
    public string? ExplainUrl { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; } = true;
}