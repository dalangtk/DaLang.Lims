
namespace DaLang.Lims.Pathology.Contracts.ExamPathologySamplingSpot.Dto;

/// <summary>
/// 病理检测取材部位-标本类型查询输出
/// </summary>
public partial class ExamPathologySamplingSpotDto : ExamPathologySamplingSpotUpdateInput
{
    public string? SamplingSpotName { get; set; }
    public string? SampleTypeName { get; set; }
}