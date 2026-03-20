namespace DaLang.Lims.Pretreatment.Contracts.BaseSorterShelfRule.Dto;

public class BaseSorterShelfRuleAddInput
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
    ///规则代码
    ///</summary>
    public string? SortRuleCode { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int? Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
