using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.Combo;

/// <summary>
/// 套餐明细 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "base_combo_detail")]
public partial class BaseComboDetailEntity : EntityTenant
{
    /// <summary>
    /// 套餐代码
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ComboCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string ComboCode { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string PurCode { get; set; }
    /// <summary>
    /// 目的名称
    /// </summary>
    /// <remarks>目的名称</remarks>
    [SugarColumn(ColumnName = "PurName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string PurName { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

