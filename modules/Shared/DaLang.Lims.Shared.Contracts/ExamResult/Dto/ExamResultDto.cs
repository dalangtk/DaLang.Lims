namespace DaLang.Lims.Shared.Contracts.ExamResult.Dto;

/// <summary>
/// 检验结果查询结果输出
/// </summary>
public class ExamResultDto : ExamResultUpdateInput
{
    public string DisplayReference => string.IsNullOrWhiteSpace(base.DisplayRange) ? base.ItemReference : base.DisplayRange;
    public bool IsCriticalValue => HLFlag == "HH" || HLFlag == "LL";
    public string JudgeCondition { get; set; }
}
