using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.ReportTemplate.Contracts.ReportTemplate.Dto;

public class ReportTemplateAddInput
{
    /// <summary>
    /// 模板代码
    /// </summary>
    [Required(ErrorMessage = "模板代码不能为空")]
    public required string TemplateCode { get; set; }
    /// <summary>
    /// 模板名称
    /// </summary>
    [Required(ErrorMessage = "模板名称不能为空")]
    public required string TemplateName { get; set; }
    /// <summary>
    ///描述
    ///</summary>
    public string? TemplateDescription { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string? GroupCode { get; set; }
    /// <summary>
    ///客户代码
    ///</summary>
    public string? HospitalCode { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string? ItemCodes { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string? PurCodes { get; set; }
    /// <summary>
    ///样本号
    ///</summary>
    public string? SampleNoSymbols { get; set; }
    /// <summary>
    ///性别代码
    ///</summary>
    public string? GenderCode { get; set; }
    /// <summary>
    ///起始时间
    ///</summary>
    public DateTime? BeginTime { get; set; }
    /// <summary>
    ///截止时间
    ///</summary>
    public DateTime? EndTime { get; set; }
    /// <summary>
    ///子报表
    ///</summary>
    public bool? IsSubTemplate { get; set; }
    /// <summary>
    ///模板类型 0其他模板 1报告模板
    ///</summary>
    public int? TemplateType { get; set; }
    /// <summary>
    ///模板内容
    ///</summary>
    public string? TemplateContent { get; set; }
    /// <summary>
    /// 数据源
    /// </summary>
    public string? TemplateDataSource { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
