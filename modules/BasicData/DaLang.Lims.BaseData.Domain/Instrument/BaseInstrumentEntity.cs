using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.Instrument;

/// <summary>
/// 仪器 实体类
/// </summary>
/// <remarks>仪器</remarks>
[SugarTable(TableName = "base_instrument")]
public partial class BaseInstrumentEntity : EntityBase
{
    /// <summary>
    /// 仪器代码
    /// </summary>
    /// <remarks>仪器代码</remarks>
    [SugarColumn(ColumnName = "InstrumentCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? InstrumentCode { get; set; }
    /// <summary>
    /// 仪器名称
    /// </summary>
    /// <remarks>仪器名称</remarks>
    [SugarColumn(ColumnName = "InstrumentName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? InstrumentName { get; set; }
    /// <summary>
    /// PinYin
    /// </summary>
    /// <remarks>PinYin</remarks>
    [SugarColumn(ColumnName = "PinYin", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? PinYin { get; set; }
    /// <summary>
    /// 五笔
    /// </summary>
    /// <remarks>五笔</remarks>
    [SugarColumn(ColumnName = "WuBi", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? WuBi { get; set; }
    /// <summary>
    /// 自定义码
    /// </summary>
    /// <remarks>自定义码</remarks>
    [SugarColumn(ColumnName = "CustomCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? CustomCode { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "Sort", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }
}

#pragma warning restore CS8618

