
namespace DaLang.Lims.Pathology.Contracts.PathologySampleType.Dto;

/// <summary>
/// 病理标本查询输出
/// </summary>
public partial class BasePathologySampleTypeDto : BasePathologySampleTypeUpdateInput
{
    public List<BasePathologySampleTypeDto> Children { get; set; } = new List<BasePathologySampleTypeDto>();
    public List<string> DiseaseCodeList
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(base.DiseaseCode))
                return base.DiseaseCode.Split(',').ToList();
            else return new List<string>();
        }
    }
    public List<string> TemplateCodeList
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(base.TemplateCode))
                return base.TemplateCode.Split(',').ToList();
            else return new List<string>();
        }
    }
}