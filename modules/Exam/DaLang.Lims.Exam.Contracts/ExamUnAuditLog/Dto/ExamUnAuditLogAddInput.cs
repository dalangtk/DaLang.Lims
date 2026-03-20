namespace DaLang.Lims.Exam.Contracts.ExamUnAuditLog.Dto;

/// <summary>
/// 反审核记录新增输入
/// </summary>
public class ExamUnAuditLogAddInput
{
    /// <summary>
    ///检验信息Id
    ///</summary>
    public long? ExamInfoId { get; set; }
    /// <summary>
    ///反审核类型
    ///</summary>
    public int? UnAuditType { get; set; }
    /// <summary>
    ///反审核原因代码
    ///</summary>
    public string? ReasonCode { get; set; }
    /// <summary>
    ///反审核原因
    ///</summary>
    public string? ReasonContent { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string? PurCodes { get; set; }
    /// <summary>
    ///目的名称
    ///</summary>
    public string? PurNames { get; set; }
}
