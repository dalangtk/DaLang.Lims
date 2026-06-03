
namespace DaLang.Lims.Pathology.Contracts.BasePathologySampleType.Dto;

/// <summary>
/// 病理标本查询输入
/// </summary>
public partial class BasePathologySampleTypeQueryInput
{
    public string? SampleTypeCode { get; set; }
    public int? TypeGrade { get; set; }
    public List<string>? SampleTypeCodes { get; set; }
}