using DaLang.Lims.Pretreatment.Core.Enum;

namespace DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;

public class SortDetailWithRule
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    ///分拣代码
    ///</summary>
    public long? SortInfoCode { get; set; }
    /// <summary>
    /// 上架模式0否 1是
    /// </summary>
    public int? IsShelfMode { get; set; } = 0;
    /// <summary>
    ///架子位置
    ///</summary>
    public int? ShelfPosition { get; set; }
    /// <summary>
    ///架子代码
    ///</summary>
    public string? ShelfCode { get; set; }
    /// <summary>
    ///架子名称
    ///</summary>
    public string? ShelfName { get; set; }
    /// <summary>
    ///架子类型 0常规1分血
    ///</summary>
    public PretreatShelfTypeEnum ShelfType { get; set; }
    /// <summary>
    ///行孔数
    ///</summary>
    public int? RowHoleCount { get; set; }
    /// <summary>
    ///列孔数
    ///</summary>
    public int? ColumnHoleCount { get; set; }
    /// <summary>
    ///使用数
    ///</summary>
    public int? UsedCount { get; set; }
    /// <summary>
    /// 架子规则明细
    /// </summary>
    public List<DetailShelfRule> ShelfRuleList = new List<DetailShelfRule>();
}

/// <summary>
/// 架子规则
/// </summary>
public class DetailShelfRule
{
    /// <summary>
    /// 架子位置
    /// </summary>
    public int ShelfPosition { get; set; }
    /// <summary>
    /// 分拣规则代码
    /// </summary>
    public string RuleCode { get; set; }
    /// <summary>
    /// 分拣规则名称
    /// </summary>
    public string RuleName { get; set; }
    /// <summary>
    /// 规则描述
    /// </summary>
    public string RuleExpression { get; set; }
    /// <summary>
    /// 序列代码
    /// </summary>
    public string SequenceCode { get; set; }
    /// <summary>
    /// 序列名称
    /// </summary>
    public string SequenceName { get; set; }
    /// <summary>
    /// 项目数量
    /// </summary>
    public int ItemCount { get; set; }
    /// <summary>
    /// 分拣所有
    /// </summary>
    public bool IsSortAll { get; set; }
    /// <summary>
    /// 是否批量
    /// </summary>
    public bool IsBatch { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
}
