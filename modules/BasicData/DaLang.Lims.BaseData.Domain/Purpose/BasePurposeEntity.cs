using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.Purpose;

/// <summary>
/// 检验目的 实体类
/// </summary>
/// <remarks>检验目的</remarks>
[SugarTable(TableName = "base_purpose")]
public partial class BasePurposeEntity : EntityBase
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
    [SugarColumn(ColumnName = "GroupName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string GroupName { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCode", ColumnDataType = "varchar", Length = 16)]
    public string PurCode { get; set; }
    /// <summary>
    /// 目的名称
    /// </summary>
    /// <remarks>目的名称</remarks>
    [SugarColumn(ColumnName = "PurName", ColumnDataType = "varchar", Length = 64)]
    public string PurName { get; set; }
    /// <summary>
    /// 目的简写
    /// </summary>
    /// <remarks>目的简写</remarks>
    [SugarColumn(ColumnName = "PurNameAB", ColumnDataType = "varchar", Length = 16)]
    public string? PurNameAB { get; set; }
    /// <summary>
    /// 目的英文
    /// </summary>
    /// <remarks>目的英文</remarks>
    [SugarColumn(ColumnName = "PurNameEN", ColumnDataType = "varchar", Length = 32)]
    public string? PurNameEN { get; set; }
    /// <summary>
    /// 标本类型代码
    /// </summary>
    /// <remarks>标本类型代码</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", ColumnDataType = "varchar", Length = 8)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 标本类型名称
    /// </summary>
    /// <remarks>标本类型名称</remarks>
    [SugarColumn(ColumnName = "SampleTypeName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SampleTypeName { get; set; }
    /// <summary>
    /// 临床意义
    /// </summary>
    /// <remarks>临床意义</remarks>
    [SugarColumn(ColumnName = "ClinicalSence", ColumnDataType = "varchar", Length = 256)]
    public string? ClinicalSence { get; set; }
    /// <summary>
    /// 建议与解释
    /// </summary>
    /// <remarks>建议与解释</remarks>
    [SugarColumn(ColumnName = "Suggestions", ColumnDataType = "varchar", Length = 256)]
    public string? Suggestions { get; set; }
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

