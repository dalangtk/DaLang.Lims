namespace DaLang.Lims.Shared.Contracts.ExamTask.Dto;

/// <summary>
/// 检验任务查询输入
/// </summary>
public class ExamTaskQueryInput
{
    public int Status { get; set; } = 0;
    public string? GroupCode { get; set; }
    public string? Barcode { get; set; }
    public DateTime? BeginDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? SampleTypeCode { get; set; }
}
