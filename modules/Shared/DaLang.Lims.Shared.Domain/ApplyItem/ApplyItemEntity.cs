using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Shared.Domain.ApplyItem;

/// <summary>
/// 申请单项目 实体类
/// </summary>
/// <remarks>申请项目</remarks>
[SugarTable(TableName = "apply_item")]
public partial class ApplyItemEntity : EntityTenant
{
    /// <summary>
    /// 目的Id
    /// </summary>
    /// <remarks>目的Id</remarks>
    [SugarColumn(ColumnName = "ApplyPurposeId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long ApplyPurposeId { get; set; }
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string GroupCode { get; set; }
    /// <summary>
    /// 组别名称
    /// </summary>
    /// <remarks>组别名称</remarks>
    [SugarColumn(ColumnName = "GroupName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string GroupName { get; set; }
    /// <summary>
    /// 条码
    /// </summary>
    /// <remarks>条码</remarks>
    [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 套餐代码
    /// </summary>
    /// <remarks>套餐代码</remarks>
    [SugarColumn(ColumnName = "ComboCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ComboCode { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string PurCode { get; set; }
    /// <summary>
    /// 上机项目代码
    /// </summary>
    /// <remarks>上机项目代码</remarks>
    [SugarColumn(ColumnName = "InstrumentItemCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? InstrumentItemCode { get; set; }
    /// <summary>
    /// 项目代码
    /// </summary>
    /// <remarks>项目代码</remarks>
    [SugarColumn(ColumnName = "ItemCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string ItemCode { get; set; }
    /// <summary>
    /// 项目名称
    /// </summary>
    /// <remarks>项目名称</remarks>
    [SugarColumn(ColumnName = "ItemName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string ItemName { get; set; }
    /// <summary>
    /// 个性化项目名称
    /// </summary>
    /// <remarks>个性化项目名称</remarks>
    [SugarColumn(ColumnName = "ItemNamePersonalize", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ItemNamePersonalize { get; set; }
}

#pragma warning restore CS8618

