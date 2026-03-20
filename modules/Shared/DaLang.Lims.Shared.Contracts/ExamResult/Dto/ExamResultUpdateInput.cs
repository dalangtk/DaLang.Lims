namespace DaLang.Lims.Shared.Contracts.ExamResult.Dto;

/// <summary>
/// 检验结果更新数据输入
/// </summary>
public class ExamResultUpdateInput : ExamResultAddInput
{
    public long Id { get; set; }
}
