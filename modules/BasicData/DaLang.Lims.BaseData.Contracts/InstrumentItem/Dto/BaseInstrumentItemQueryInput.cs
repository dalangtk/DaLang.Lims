namespace DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;

/// <summary>
/// 上机项目分页查询条件输入
/// </summary>
public class BaseInstrumentItemQueryInput
{

    /// <summary>组别代码</summary>       
    public string? GroupCode { get; set; }
    /// <summary>上机项目代码</summary>       
    public string? InstrumentItemCode { get; set; }
}
