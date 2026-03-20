using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.Purpose;

/// <summary>
/// 目的定制 实体类
/// </summary>
/// <remarks>目的定制</remarks>
[SugarTable(TableName = "base_purpose_personalize")]
public partial class BasePurposePersonalizeEntity : EntityTenant
{
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string PurCode { get; set; }
    /// <summary>
    /// 标本类型
    /// </summary>
    /// <remarks>标本类型</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", ColumnDataType = "varchar", Length = 16)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 个性化目的名称
    /// </summary>
    /// <remarks>个性化目的名称</remarks>
    [SugarColumn(ColumnName = "PurNamePersonalize", ColumnDataType = "varchar", Length = 32)]
    public string? PurNamePersonalize { get; set; }
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", ColumnDataType = "varchar", Length = 16)]
    public string? CustomerCode { get; set; }
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

    /// <summary>
    /// 标本类型名称
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public string SampleTypeName { get; set; }
}

#pragma warning restore CS8618

