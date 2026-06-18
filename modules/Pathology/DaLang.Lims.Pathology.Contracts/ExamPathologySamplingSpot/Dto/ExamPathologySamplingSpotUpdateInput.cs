
namespace DaLang.Lims.Pathology.Contracts.ExamPathologySamplingSpot.Dto;

/// <summary>
/// 病理检测取材部位-标本类型更新数据输入
/// </summary>
public partial class ExamPathologySamplingSpotUpdateInput : ExamPathologySamplingSpotAddInput
{
    public long Id { get; set; }
}