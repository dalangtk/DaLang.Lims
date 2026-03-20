namespace DaLang.Lims.Exam.Contracts.ExamCriticalValue.Dto;

/// <summary>
/// 危急值新增输入
/// </summary>
public class ExamCriticalValueAddInput
{
    /// <summary>
    ///组别代码
    ///</summary>
    public string? GroupCode { get; set; }
    /// <summary>
    ///组别名称
    ///</summary>
    public string? GroupName { get; set; }
    /// <summary>
    ///检验Id
    ///</summary>
    public long? ExamInfoId { get; set; }
    /// <summary>
    ///结果Id
    ///</summary>
    public long? ExamResultId { get; set; }
    /// <summary>
    ///条码
    ///</summary>
    public string Barcode { get; set; }
    /// <summary>
    ///样本号
    ///</summary>
    public string SampleNo { get; set; }
    /// <summary>
    ///检测日期
    ///</summary>
    public DateTime? TestDate { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string PurCode { get; set; }
    /// <summary>
    ///目的名称
    ///</summary>
    public string? PurName { get; set; }
    /// <summary>
    ///上机项目代码
    ///</summary>
    public string? InstrumentItemCode { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string ItemCode { get; set; }
    /// <summary>
    ///项目名称
    ///</summary>
    public string ItemName { get; set; }
    /// <summary>
    ///检验结果
    ///</summary>
    public string? ItemResult { get; set; }
    /// <summary>
    ///危急值内容
    ///</summary>
    public string? CriticalContent { get; set; }
    /// <summary>
    ///复查时间
    ///</summary>
    public DateTime? ReviewTime { get; set; }
    /// <summary>
    ///复查结果
    ///</summary>
    public string? ReviewResult { get; set; }
    /// <summary>
    ///联系人
    ///</summary>
    public string? ContactName { get; set; }
    /// <summary>
    ///联系电话
    ///</summary>
    public string? ContactPhone { get; set; }
    /// <summary>
    ///联系时间
    ///</summary>
    public DateTime? ContactTime { get; set; }
    /// <summary>
    ///复述内容
    ///</summary>
    public string? RepeatContent { get; set; }
    /// <summary>
    ///处理情况及反馈
    ///</summary>
    public string? ProcessRemark { get; set; }
    /// <summary>
    ///处理人Id
    ///</summary>
    public long? ProcessId { get; set; }
    /// <summary>
    ///处理人
    ///</summary>
    public string? ProcessName { get; set; }
    /// <summary>
    ///处理时间
    ///</summary>
    public DateTime? ProcessTime { get; set; }
    /// <summary>
    ///是否取消
    ///</summary>
    public bool IsCancel { get; set; } = false;
    /// <summary>
    ///取消原因
    ///</summary>
    public string? CancelReason { get; set; }
    /// <summary>
    ///处理状态
    ///</summary>
    public int ProcessStatus { get; set; } = 0;
}
