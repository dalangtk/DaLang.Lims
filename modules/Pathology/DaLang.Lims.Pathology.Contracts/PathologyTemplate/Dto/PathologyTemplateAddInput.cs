namespace DaLang.Lims.Pathology.Contracts.PathologyTemplate.Dto;

/// <summary>
/// 诊断模板新增输入
/// </summary>
public class PathologyTemplateAddInput
{
    /// <summary>
    ///工作流
    ///</summary>
    public string? WFCode { get; set; }
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
    /// <summary>
    ///诊断意见
    ///</summary>
    public string? DiagnosisResult { get; set; }
    /// <summary>
    ///备注
    ///</summary>
    public string? Remark { get; set; }
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCodes { get; set; }
    /// <summary>
    ///客户类型
    ///</summary>
    public string? CustomerType { get; set; }
    /// <summary>
    ///模板类型 1诊断 2巨检
    ///</summary>
    public int? TemplateType { get; set; }
    /// <summary>
    ///默认结果
    ///</summary>
    public bool? IsDefaultResult { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;

}
