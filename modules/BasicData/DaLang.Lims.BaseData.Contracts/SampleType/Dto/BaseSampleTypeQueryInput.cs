namespace DaLang.Lims.BaseData.Contracts.SampleType.Dto;

/// <summary>
/// 标本类型分页查询条件输入
/// </summary>
public partial class BaseSampleTypeQueryInput
{

    /// <summary>
    /// 标本类型代码
    /// </summary>       
    public string? SampleTypeCode { get; set; }
}
