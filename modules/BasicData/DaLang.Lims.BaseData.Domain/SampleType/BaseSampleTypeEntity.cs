using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.SampleType;

/// <summary>
/// 标本类型 实体类
/// </summary>
/// <remarks>标本类型</remarks>
[SugarTable(TableName = "base_sample_type")]
public partial class BaseSampleTypeEntity : EntityBase
{
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
    [SugarColumn(ColumnName = "SampleTypeName", ColumnDataType = "varchar", Length = 32)]
    public string? SampleTypeName { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    /// <remarks>性别</remarks>
    [SugarColumn(ColumnName = "Gender", ColumnDataType = "varchar", Length = 16)]
    public string Gender { get; set; }
    /// <summary>
    /// 拼音
    /// </summary>
    /// <remarks>拼音</remarks>
    [SugarColumn(ColumnName = "PinYin", ColumnDataType = "varchar", Length = 16)]
    public string? PinYin { get; set; }
    /// <summary>
    /// 五笔
    /// </summary>
    /// <remarks>五笔</remarks>
    [SugarColumn(ColumnName = "WuBi", ColumnDataType = "varchar", Length = 16)]
    public string? WuBi { get; set; }
    /// <summary>
    /// 自定义码
    /// </summary>
    /// <remarks>自定义码</remarks>
    [SugarColumn(ColumnName = "CustomCode", ColumnDataType = "varchar", Length = 16)]
    public string? CustomCode { get; set; }
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

