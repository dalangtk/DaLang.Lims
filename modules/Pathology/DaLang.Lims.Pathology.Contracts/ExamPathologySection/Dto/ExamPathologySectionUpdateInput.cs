
namespace DaLang.Lims.Pathology.Contracts.ExamPathologySection.Dto;

/// <summary>
/// 切片更新数据输入
/// </summary>
public partial class ExamPathologySectionUpdateInput : ExamPathologySectionAddInput
{
    public long Id { get; set; }
}