using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.PretreatCustomerPurposeMatch;

/// <summary>
/// 目的对照 实体类
/// </summary>
/// <remarks>目的对应</remarks>
[SugarTable(TableName = "pretreat_customer_purpose_match")]
public partial class PretreatCustomerPurposeMatchEntity : EntityTenant
{
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 客户目的代码
    /// </summary>
    /// <remarks>客户目的代码</remarks>
    [SugarColumn(ColumnName = "CustomerPurCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? CustomerPurCode { get; set; }
    /// <summary>
    /// 客户目的名称
    /// </summary>
    /// <remarks>客户目的名称</remarks>
    [SugarColumn(ColumnName = "CustomerPurName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? CustomerPurName { get; set; }
    /// <summary>
    /// 中心目的代码
    /// </summary>
    /// <remarks>中心目的代码</remarks>
    [SugarColumn(ColumnName = "CentralPurCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? CentralPurCode { get; set; }
    /// <summary>
    /// 中心目的名称
    /// </summary>
    /// <remarks>中心目的名称</remarks>
    [SugarColumn(ColumnName = "CentralPurName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? CentralPurName { get; set; }
    /// <summary>
    /// 是否套餐
    /// </summary>
    /// <remarks>是否套餐</remarks>
    [SugarColumn(ColumnName = "IsCombo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11, DefaultValue = "0")]
    public int IsCombo { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
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

