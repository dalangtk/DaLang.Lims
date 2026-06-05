
namespace DaLang.Lims.Pathology.Contracts.PathologySamplingSpotDetail.Dto;

/// <summary>
/// 取材部位明细查询输出
/// </summary>
public partial class BasePathologySamplingSpotDetailDto : BasePathologySamplingSpotDetailUpdateInput
{
    public string SamplingSpotName { get; set; }
    public string SampleTypeName { get; set; }
}