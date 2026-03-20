namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

/// <summary>
///目的明细查询结果输出
///</summary>
public partial class BasePurposeDetailDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string PurCode { get; set; }
    /// <summary>
    ///上机项目代码
    ///</summary>
    public string InstrumentItemCode { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string ItemCode { get; set; }
    /// <summary>
    ///结果类型，定量、定性等
    ///</summary>
    public string? ResultType { get; set; }
    /// <summary>
    ///方法学
    ///</summary>
    public string? Method { get; set; }
    /// <summary>
    ///报告显示
    ///</summary>
    public bool? IsReportShow { get; set; }
    /// <summary>
    /// 默认值
    /// </summary>
    public string? DefaultValue { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
    /// <summary>
    /// 项目名称
    /// </summary>
    public string? ItemName { get; set; }
    /// <summary>
    /// 打印排序
    /// </summary>
    public string? PrintOrder { get; set; }
    /// <summary>
    /// 上机项目名称
    /// </summary>
    public string? InstrumentItemName { get; set; }
}
