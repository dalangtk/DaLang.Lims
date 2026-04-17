namespace DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;

#pragma warning disable CS8618

/// <summary>
///上机项目查询结果输出
///</summary>
public class BaseInstrumentItemDto
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
    ///组别名称
    ///</summary>
    public string GroupName { get; set; }
    /// <summary>
    ///上机项目代码
    ///</summary>
    public string? InstrumentItemCode { get; set; }
    /// <summary>
    ///上机项目名称
    ///</summary>
    public string InstrumentItemName { get; set; }
    /// <summary>
    ///打印排序
    ///</summary>
    public string PrintOrder { get; set; }
    /// <summary>
    ///备注
    ///</summary>
    public string? Remark { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
}
#pragma warning restore CS8618