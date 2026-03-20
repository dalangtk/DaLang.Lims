using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.BaseSorterShelfRule;

/// <summary>
/// 架子规则 实体类
/// </summary>
/// <remarks>分拣架规则</remarks>
[SugarTable(TableName = "base_sorter_shelf_rule")]
public partial class BaseSorterShelfRuleEntity : EntityTenant
{
    /// <summary>
    /// 分拣仪代码
    /// </summary>
    /// <remarks>分拣仪代码</remarks>
    [SugarColumn(ColumnName = "SorterCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? SorterCode { get; set; }
    /// <summary>
    /// 架子名称
    /// </summary>
    /// <remarks>架子名称</remarks>
    [SugarColumn(ColumnName = "ShelfName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ShelfName { get; set; }
    /// <summary>
    /// 架子位置
    /// </summary>
    /// <remarks>架子位置</remarks>
    [SugarColumn(ColumnName = "ShelfPosition", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 9)]
    public int? ShelfPosition { get; set; }
    /// <summary>
    /// 架子类型
    /// </summary>
    /// <remarks>架子类型</remarks>
    [SugarColumn(ColumnName = "ShelfType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? ShelfType { get; set; }
    /// <summary>
    /// 规则代码
    /// </summary>
    /// <remarks>规则代码</remarks>
    [SugarColumn(ColumnName = "SortRuleCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? SortRuleCode { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

