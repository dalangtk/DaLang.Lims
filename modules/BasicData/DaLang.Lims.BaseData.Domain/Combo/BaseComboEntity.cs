using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.Combo;

/// <summary>
/// 套餐管理 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName="base_combo")]
public partial class BaseComboEntity: EntityTenant
{
    /// <summary>
    /// 套餐代码
    /// </summary>
    /// <remarks>套餐代码</remarks>
    [SugarColumn(ColumnName="ComboCode", ColumnDataType="varchar", Length=16)]
    public string ComboCode { get; set; }
    /// <summary>
    /// 套餐名称
    /// </summary>
    /// <remarks>套餐名称</remarks>
    [SugarColumn(ColumnName="ComboName", ColumnDataType="varchar", Length=32)]
    public string ComboName { get; set; }
    /// <summary>
    /// 客户
    /// </summary>
    /// <remarks>客户</remarks>
    [SugarColumn(ColumnName="CustomerCode", ColumnDataType="varchar", Length=16)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 起始时间
    /// </summary>
    /// <remarks>起始时间</remarks>
    [SugarColumn(ColumnName="BeginDate", ColumnDataType="datetime")]
    public DateTime? BeginDate { get; set; }
    /// <summary>
    /// 截至时间
    /// </summary>
    /// <remarks>截至时间</remarks>
    [SugarColumn(ColumnName="EndDate", ColumnDataType="datetime")]
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 标本类型
    /// </summary>
    /// <remarks>标本类型</remarks>
    [SugarColumn(ColumnName="SampleTypeCode", ColumnDataType="varchar", Length=16)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName="Remark", ColumnDataType="varchar", Length=256)]
    public string? Remark { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName="Sort", ColumnDataType="int", DecimalDigits = 11)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName="IsValid", ColumnDataType="tinyint", DefaultValue="true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

