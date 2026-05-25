namespace DaLang.Lims.Pathology.Contracts.PathologySamplingSpot.Dto;

/// <summary>
/// 取材部位更新输入
/// </summary>
public class SamplingSpotUpdateInput : SamplingSpotAddInput
{
    public long Id { get; set; }
}
