using DaLang.Lims.Web.Common.Enums;

namespace DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;

/// <summary>
/// 样本跟踪新增输入
/// </summary>
public class ExamSampleTrackAddInput
{
    /// <summary>
    ///条码
    ///</summary>
    public string Barcode { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///组别名称
    ///</summary>
    public string GroupName { get; set; }
    /// <summary>
    /// 检测日期
    /// </summary>
    public DateTime? TestDate { get; set; }
    /// <summary>
    ///流水号
    ///</summary>
    public string? SampleNo { get; set; }
    /// <summary>
    ///记录内容
    ///</summary>
    public string? TrackContent { get; set; }
    /// <summary>
    ///操作类型
    ///</summary>
    public OperationTypeEnum OperationType { get; set; }

}
