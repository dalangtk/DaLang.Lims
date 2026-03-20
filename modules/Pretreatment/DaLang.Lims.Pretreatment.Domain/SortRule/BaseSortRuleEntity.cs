using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.SortRule;

/// <summary>
/// 分拣规则 实体类
/// </summary>
/// <remarks>分拣规则</remarks>
[SugarTable(TableName = "base_sort_rule")]
public partial class BaseSortRuleEntity : EntityTenant
{
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", ColumnDataType = "varchar", Length = 16)]
    public string GroupCode { get; set; }
    /// <summary>
    /// 组别名称
    /// </summary>
    /// <remarks>组别名称</remarks>
    [SugarColumn(ColumnName = "GroupName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string GroupName { get; set; }
    /// <summary>
    /// 规则代码
    /// </summary>
    /// <remarks>规则代码</remarks>
    [SugarColumn(ColumnName = "RuleCode", ColumnDataType = "varchar", Length = 16)]
    public string RuleCode { get; set; }
    /// <summary>
    /// 规则名称
    /// </summary>
    /// <remarks>规则名称</remarks>
    [SugarColumn(ColumnName = "RuleName", ColumnDataType = "varchar", Length = 32)]
    public string RuleName { get; set; }
    /// <summary>
    /// 规则条件
    /// </summary>
    /// <remarks>规则条件</remarks>
    [SugarColumn(ColumnName = "RuleExpression", ColumnDataType = "varchar", Length = 512)]
    public string? RuleExpression { get; set; }
    /// <summary>
    /// 序列代码
    /// </summary>
    /// <remarks>序列代码</remarks>
    [SugarColumn(ColumnName = "SequenceCode", ColumnDataType = "varchar", Length = 8)]
    public string? SequenceCode { get; set; }
    /// <summary>
    /// 匹配项目数
    /// </summary>
    /// <remarks>匹配项目数</remarks>
    [SugarColumn(ColumnName = "ItemCount", ColumnDataType = "int", DecimalDigits = 11)]
    public int? ItemCount { get; set; }
    /// <summary>
    /// 是否分拣所有
    /// </summary>
    /// <remarks>是否分拣所有</remarks>
    [SugarColumn(ColumnName = "IsSortAll", ColumnDataType = "tinyint")]
    public bool IsSortAll { get; set; }
    /// <summary>
    /// 是否批量规则
    /// </summary>
    /// <remarks>是否批量规则</remarks>
    [SugarColumn(ColumnName = "IsBatch", ColumnDataType = "tinyint")]
    public bool IsBatch { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

