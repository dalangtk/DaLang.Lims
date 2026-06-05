
namespace DaLang.Lims.Pathology.Contracts.PathologySamplingSpotDetail.Dto;

/// <summary>
/// 取材部位明细新增输入
/// </summary>
public partial class BasePathologySamplingSpotDetailAddInput
{
    public string SamplingSpotCode { get; set; }
    public string SampleTypeCode { get; set; }
}