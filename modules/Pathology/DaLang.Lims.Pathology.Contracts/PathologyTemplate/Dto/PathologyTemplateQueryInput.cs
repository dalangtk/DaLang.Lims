namespace DaLang.Lims.Pathology.Contracts.PathologyTemplate.Dto;

/// <summary>
/// 诊断模板查询输入
/// </summary>
public class PathologyTemplateQueryInput
{
    public string? TemplateCode { get; set; }
    public string WFCode { get; set; }
    public int? TemplateType { get; set; }
}
