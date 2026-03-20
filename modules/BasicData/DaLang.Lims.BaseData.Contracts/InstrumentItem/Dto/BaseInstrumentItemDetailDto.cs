namespace DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;

public class BaseInstrumentItemDetailDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///上机项目代码
    ///</summary>
    public string InstrumentItemCode { get; set; }
    /// <summary>
    /// 上机项目名称
    /// </summary>
    public string InstrumentItemName { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string? ItemCode { get; set; }
    /// <summary>
    /// 项目名称
    /// </summary>
    public string? ItemName { get; set; }
    /// <summary>
    /// 结果类型
    /// </summary>
    public string? ResultType { get; set; }
    /// <summary>
    /// 打印排序
    /// </summary>
    public string? PrintOrder { get; set; }
    /// <summary>
    /// 方法学
    /// </summary>
    public string? Method { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
}
