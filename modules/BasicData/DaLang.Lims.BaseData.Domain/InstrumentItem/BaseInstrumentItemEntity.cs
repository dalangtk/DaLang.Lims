using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.InstrumentItem;

/// <summary>
/// 上机项目 实体类
/// </summary>
/// <remarks>上机项目</remarks>
[SugarTable(TableName = "base_instrument_item")]
public partial class BaseInstrumentItemEntity : EntityBase
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
    [SugarColumn(ColumnName = "GroupName", ColumnDataType = "varchar", Length = 32)]
    public string GroupName { get; set; }
    /// <summary>
    /// 上机项目代码
    /// </summary>
    /// <remarks>上机项目代码</remarks>
    [SugarColumn(ColumnName = "InstrumentItemCode", ColumnDataType = "varchar", Length = 16)]
    public string InstrumentItemCode { get; set; }
    /// <summary>
    /// 上机项目名称
    /// </summary>
    /// <remarks>上机项目名称</remarks>
    [SugarColumn(ColumnName = "InstrumentItemName", ColumnDataType = "varchar", Length = 32)]
    public string InstrumentItemName { get; set; }
    /// <summary>
    /// 打印排序
    /// </summary>
    /// <remarks>打印排序</remarks>
    [SugarColumn(ColumnName = "PrintOrder", ColumnDataType = "varchar", Length = 8)]
    public string PrintOrder { get; set; }
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

