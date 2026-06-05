
namespace DaLang.Lims.Pathology.Contracts.PathologySamplingSpotDetail.Dto;

/// <summary>
/// 取材部位明细更新数据输入
/// </summary>
public partial class BasePathologySamplingSpotDetailUpdateInput : BasePathologySamplingSpotDetailAddInput
{
    public long Id { get; set; }
}