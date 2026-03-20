using DaLang.Lims.Pretreatment.Core.Enum;
using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.BaseSorterShelf;

/// <summary>
/// 分拣架子 实体类
/// </summary>
/// <remarks>分拣架</remarks>
[SugarTable(TableName = "base_sorter_shelf")]
public partial class BaseSorterShelfEntity : EntityTenant
{
    /// <summary>
    /// 分拣仪代码
    /// </summary>
    /// <remarks>分拣仪代码</remarks>
    [SugarColumn(ColumnName = "SorterCode", ColumnDataType = "varchar", Length = 8)]
    public string? SorterCode { get; set; }
    /// <summary>
    /// 架子名称
    /// </summary>
    /// <remarks>架子名称</remarks>
    [SugarColumn(ColumnName = "ShelfName", ColumnDataType = "varchar", Length = 32)]
    public string? ShelfName { get; set; }
    /// <summary>
    /// 架子位置
    /// </summary>
    /// <remarks>架子位置</remarks>
    [SugarColumn(ColumnName = "ShelfPosition", ColumnDataType = "int", DecimalDigits = 9)]
    public int? ShelfPosition { get; set; }
    /// <summary>
    /// 架子类型
    /// </summary>
    /// <remarks>架子类型</remarks>
    [SugarColumn(ColumnName = "ShelfType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int")]
    public PretreatShelfTypeEnum ShelfType { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }
}

#pragma warning restore CS8618

