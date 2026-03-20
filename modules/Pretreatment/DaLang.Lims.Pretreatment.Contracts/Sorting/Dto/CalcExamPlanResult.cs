namespace DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;

/// <summary>
/// 计算检测计划结果
/// </summary>
public class CalcExamPlanResult
{
    public string ExamPlanCode { get; set; }
    public TimeSpan ReceiveTimePoint { get; set; }
    public DateTime ReceiveDate { get; set; }
    public DateTime? TestDate { get; set; }
    public DateTime? ReportTime { get; set; }
}
