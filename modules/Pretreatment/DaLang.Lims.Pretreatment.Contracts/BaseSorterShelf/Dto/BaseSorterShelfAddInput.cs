namespace DaLang.Lims.Pretreatment.Contracts.BaseSorterShelf.Dto;

/// <summary>
/// 架子规则新增输入
/// </summary>
public partial class BaseSorterShelfAddInput
{
    /// <summary>
    ///分拣仪代码
    ///</summary>
    public string? SorterCode { get; set; }
    /// <summary>
    ///架子名称
    ///</summary>
    public string? ShelfName { get; set; }
    /// <summary>
    ///架子位置
    ///</summary>
    public int? ShelfPosition { get; set; }
    /// <summary>
    ///架子类型
    ///</summary>
    public int? ShelfType { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
}
