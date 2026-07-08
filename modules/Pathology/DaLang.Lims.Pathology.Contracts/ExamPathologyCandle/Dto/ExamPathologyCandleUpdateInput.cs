
namespace DaLang.Lims.Pathology.Contracts.ExamPathologyCandle.Dto;

/// <summary>
/// 蜡块更新数据输入
/// </summary>
public partial class ExamPathologyCandleUpdateInput : ExamPathologyCandleAddInput
{
    public long Id { get; set; }
}