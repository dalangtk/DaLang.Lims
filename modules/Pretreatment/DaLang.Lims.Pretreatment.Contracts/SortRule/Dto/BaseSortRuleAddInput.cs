using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.SortRule.Dto;

/// <summary>
/// 分拣规则新增输入
/// </summary>
public class BaseSortRuleAddInput
{
    /// <summary>组别代码</summary>
    public string GroupCode { get; set; }
    /// <summary>组别名称</summary>
    public string GroupName { get; set; }
    /// <summary>规则代码</summary>
    public string RuleCode { get; set; }
    /// <summary>规则名称</summary>
    public string RuleName { get; set; }
    /// <summary>规则条件</summary>
    public string? RuleExpression { get; set; }
    public DynamicFilterInfo? RuleExpressionObj { get; set; }
    /// <summary>序列代码</summary>
    public string? SequenceCode { get; set; }
    /// <summary>匹配项目数</summary>
    public int? ItemCount { get; set; }
    /// <summary>是否分拣所有</summary>
    public bool IsSortAll { get; set; }
    /// <summary>是否批量规则</summary>
    public bool IsBatch { get; set; }
    /// <summary>排序</summary>
    public int Sort { get; set; }
    /// <summary>启用</summary>
    public bool IsValid { get; set; } = true;
}
