
namespace DaLang.Lims.Pathology.Contracts.ExamPathologySamplingSpot.Dto;

/// <summary>
/// 病理检测取材部位-标本类型新增输入
/// </summary>
public partial class ExamPathologySamplingSpotAddInput {
    /// <summary>
    /// 
    /// </summary>
    public long? ExamInfoId { get; set; }                                                    
    /// <summary>
    /// 采样部位代码
    /// </summary>
    public string? SamplingSpotCode { get; set; }                                                    
    /// <summary>
    /// 标本类型代码
    /// </summary>
    public string? SampleTypeCode { get; set; }                                                    
}