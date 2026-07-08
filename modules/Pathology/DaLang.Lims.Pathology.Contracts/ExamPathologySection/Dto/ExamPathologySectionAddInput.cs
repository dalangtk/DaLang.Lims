
namespace DaLang.Lims.Pathology.Contracts.ExamPathologySection.Dto;

/// <summary>
/// 切片新增输入
/// </summary>
public partial class ExamPathologySectionAddInput
{
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 蜡块Id
    /// </summary>
    public long? CandleId { get; set; }
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
    /// 评估
    /// </summary>
    public string? Assessment { get; set; }
    /// <summary>
    /// 反馈
    /// </summary>
    public string? FeedBack { get; set; }
    /// <summary>
    /// 是否临时医嘱
    /// </summary>
    public bool? IsMedicalAdvice { get; set; }
    /// <summary>
    /// 临时医嘱发起人
    /// </summary>
    public long? MedicalAdviceUser { get; set; }
}