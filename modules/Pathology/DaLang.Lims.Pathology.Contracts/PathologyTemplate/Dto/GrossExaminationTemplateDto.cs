namespace DaLang.Lims.Pathology.Contracts.PathologyTemplate.Dto;

public class GrossExaminationTemplateDto
{
    public string SampleTypeName { get; set; }
    /// <summary>
    ///模板代码
    ///</summary>
    public string? TemplateCode { get; set; }
    /// <summary>
    ///模板名称
    ///</summary>
    public string? TemplateName { get; set; }
    /// <summary>
    ///模板内容
    ///</summary>
    public string? TemplateContent { get; set; }
}
