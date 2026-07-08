
namespace DaLang.Lims.Pathology.Contracts.ExamPathologyCandle.Dto;

/// <summary>
/// 蜡块新增输入
/// </summary>
public partial class ExamPathologyCandleAddInput
{
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 蜡块号
    /// </summary>
    public int? CandleNo { get; set; }
    /// <summary>
    /// 原蜡块号
    /// </summary>
    public string? OriginalCandleNo { get; set; }
    /// <summary>
    /// 部位
    /// </summary>
    public string? Position { get; set; }
    /// <summary>
    /// 数量
    /// </summary>
    public int? Amount { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    public int? OperationType { get; set; }
    /// <summary>
    /// 取材时间
    /// </summary>
    public DateTime? EstimatedSamplingDate { get; set; }
}