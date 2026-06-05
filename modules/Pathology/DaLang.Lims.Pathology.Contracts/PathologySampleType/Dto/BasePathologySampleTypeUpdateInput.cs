
namespace DaLang.Lims.Pathology.Contracts.PathologySampleType.Dto;

/// <summary>
/// 病理标本更新数据输入
/// </summary>
public partial class BasePathologySampleTypeUpdateInput : BasePathologySampleTypeAddInput
{
    public long Id { get; set; }
}