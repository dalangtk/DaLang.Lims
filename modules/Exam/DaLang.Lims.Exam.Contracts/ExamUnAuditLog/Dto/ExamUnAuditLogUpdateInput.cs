namespace DaLang.Lims.Exam.Contracts.ExamUnAuditLog.Dto;

/// <summary>
/// 反审核记录更新输入
/// </summary>
public class ExamUnAuditLogUpdateInput : ExamUnAuditLogAddInput
{
    public long Id { get; set; }
}
