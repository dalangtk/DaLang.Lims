using DaLang.Lims.Pretreatment.Core.Enum;
using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.Sorting;

/// <summary>
/// 基础数据 实体类
/// </summary>
/// <remarks>分拣信息详情</remarks>
[SugarTable(TableName = "pretreat_sort_detail")]
public partial class PretreatSortDetailEntity : EntityTenant
{
    /// <summary>
    /// 分拣代码
    /// </summary>
    /// <remarks>分拣代码</remarks>
    [SugarColumn(ColumnName = "SortInfoCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? SortInfoCode { get; set; }
    /// <summary>
    /// 架子位置
    /// </summary>
    /// <remarks>架子位置</remarks>
    [SugarColumn(ColumnName = "ShelfPosition", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? ShelfPosition { get; set; }
    /// <summary>
    /// 架子代码
    /// </summary>
    /// <remarks>架子代码</remarks>
    [SugarColumn(ColumnName = "ShelfCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ShelfCode { get; set; }
    /// <summary>
    /// 架子名称
    /// </summary>
    /// <remarks>架子名称</remarks>
    [SugarColumn(ColumnName = "ShelfName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ShelfName { get; set; }
    /// <summary>
    /// 架子类型 0常规1分血
    /// </summary>
    /// <remarks>架子类型</remarks>
    [SugarColumn(ColumnName = "ShelfType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int")]
    public PretreatShelfTypeEnum ShelfType { get; set; }
    /// <summary>
    /// 行孔数
    /// </summary>
    /// <remarks>行孔数</remarks>
    [SugarColumn(ColumnName = "RowHoleCount", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? RowHoleCount { get; set; }
    /// <summary>
    /// 列孔数
    /// </summary>
    /// <remarks>列孔数</remarks>
    [SugarColumn(ColumnName = "ColumnHoleCount", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? ColumnHoleCount { get; set; }
    /// <summary>
    /// 使用数
    /// </summary>
    /// <remarks>使用数</remarks>
    [SugarColumn(ColumnName = "UsedCount", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? UsedCount { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? Sort { get; set; }
}

#pragma warning restore CS8618

