
namespace DaLang.Lims.Pathology.Contracts.BasePathologySampleType.Dto;

/// <summary>
/// 病理标本新增输入
/// </summary>
public partial class BasePathologySampleTypeAddInput
{
    /// <summary>
    /// 标本类型代码
    /// </summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 标本类型名称
    /// </summary>
    public string? SampleTypeName { get; set; }
    /// <summary>
    /// 父级代码
    /// </summary>
    public string? ParentCode { get; set; }
    /// <summary>
    /// 级别
    /// </summary>
    public int? TypeGrade { get; set; }
    /// <summary>
    /// 疾病代码
    /// </summary>
    public string? DiseaseCode { get; set; }
    /// <summary>
    /// 模板代码
    /// </summary>
    public string? TemplateCode { get; set; }
    /// <summary>
    /// 拼音
    /// </summary>
    public string? PinYin { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; } = true;
}