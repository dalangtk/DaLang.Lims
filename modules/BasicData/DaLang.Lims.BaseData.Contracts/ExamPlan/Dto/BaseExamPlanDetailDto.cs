namespace DaLang.Lims.BaseData.Contracts.ExamPlan.Dto;

///<summary>
///检测计划明细查询结果输出
///</summary>
public partial class BaseExamPlanDetailDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///租户Id
    ///</summary>
    public long TenantId { get; set; }
    /// <summary>
    ///检测计划代码
    ///</summary>
    public string ExamPlanCode { get; set; }
    /// <summary>
    ///接收时间
    ///</summary>
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    ///检测间隔
    ///</summary>
    public int? TestInterval { get; set; }
    /// <summary>
    ///报告间隔
    ///</summary>
    public int? ReportInterval { get; set; }
    /// <summary>
    ///报告时间
    ///</summary>
    public string? ReportTimePoint { get; set; }
    /// <summary>
    ///时间类型，点/秒/分/时/天等
    ///</summary>
    public string? TimePointType { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
}
