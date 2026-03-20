using DaLang.Lims.Web.Common.Enums;

namespace DaLang.Lims.Exam.Contracts.SampleTest.Dto;

public class AuditInput
{
    public long ExamInfoId { get; set; }
    /// <summary>
    /// 审核类型 初审/复审
    /// </summary>
    public OperationTypeEnum AuditType { get; set; } = OperationTypeEnum.FirstCheck;
    /// <summary>
    /// 执行类型 单个/批量
    /// </summary>
    public ExecuteTypeEnum ExecuteType { get; set; } = ExecuteTypeEnum.Single;
    /// <summary>
    /// 带教者Id
    /// </summary>
    public long? TeacherId { get; set; }
    /// <summary>
    /// 带教者姓名
    /// </summary>
    public string? TeacherName { get; set; }
    //public bool ForceAudit { get; set; } = false;
    public List<string>? IgnoreAuditRuleCodes { get; set; }
}

public class UnAuditInput
{
    public long ExamInfoId { get; set; }
    public string ReasonCode { get; set; }
    public string ReasonContent { get; set; }
    /// <summary>
    /// 执行类型 单个/批量
    /// </summary>
    public ExecuteTypeEnum ExecuteType { get; set; } = ExecuteTypeEnum.Single;
}
