using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.BaseSorter;

/// <summary>
/// 分拣仪器 实体类
/// </summary>
/// <remarks>分拣仪</remarks>
[SugarTable(TableName = "base_sorter")]
public partial class BaseSorterEntity : EntityTenant
{
    /// <summary>
    /// 分拣仪代码
    /// </summary>
    /// <remarks>分拣仪代码</remarks>
    [SugarColumn(ColumnName = "SorterCode", ColumnDataType = "varchar", Length = 8)]
    public string? SorterCode { get; set; }
    /// <summary>
    /// 分拣仪名称
    /// </summary>
    /// <remarks>分拣仪名称</remarks>
    [SugarColumn(ColumnName = "SorterName", ColumnDataType = "varchar", Length = 32)]
    public string? SorterName { get; set; }
    /// <summary>
    /// 架子数
    /// </summary>
    /// <remarks>架子数</remarks>
    [SugarColumn(ColumnName = "ShelfCount", ColumnDataType = "int", DecimalDigits = 8)]
    public int? ShelfCount { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
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
    public bool IsValid { get; set; }
}

#pragma warning restore CS8618

