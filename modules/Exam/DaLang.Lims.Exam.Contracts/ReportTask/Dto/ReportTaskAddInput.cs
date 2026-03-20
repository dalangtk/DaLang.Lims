namespace DaLang.Lims.Exam.Contracts.ReportTask.Dto;

/// <summary>
/// 报告任务新建输入
/// </summary>
public class ReportTaskAddInput
{
    /// <summary>
    ///检验信息Id
    ///</summary>
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 条码
    /// </summary>
    public string Barcode { get; set; }
    /// <summary>
    ///任务类型
    ///</summary>
    public int? TaskType { get; set; }
    /// <summary>
    ///任务优先级
    ///</summary>
    public int? TaskPriority { get; set; }
    /// <summary>
    ///处理状态
    ///</summary>
    public int ProcessStatus { get; set; } = 0;
    /// <summary>
    ///合并检验信息Id
    ///</summary>
    public string? ExamInfoIds { get; set; }
    /// <summary>
    ///合并信息
    ///</summary>
    public string? MergeInfo { get; set; }

}
