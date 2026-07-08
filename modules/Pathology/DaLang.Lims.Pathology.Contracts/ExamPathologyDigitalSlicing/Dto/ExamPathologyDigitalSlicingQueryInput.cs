
namespace DaLang.Lims.Pathology.Contracts.ExamPathologyDigitalSlicing.Dto;

/// <summary>
/// 数字切片查询输入
/// </summary>
public class ExamPathologyDigitalSlicingQueryInput
{
    public string? Query { get; set; }
    public long? ExamInfoId { get; set; }
}