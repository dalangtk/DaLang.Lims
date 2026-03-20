namespace DaLang.Lims.Exam.Contracts.ExamCriticalValue.Dto;

/// <summary>
/// 危急值更新数据输入
/// </summary>
public class ExamCriticalValueUpdateInput : ExamCriticalValueAddInput
{
    public long Id { get; set; }
}
