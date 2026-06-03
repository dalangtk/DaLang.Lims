using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.BasePathologySampleType;

/// <summary>
/// 病理标本 实体类
/// </summary>
/// <remarks>病理标本类型</remarks>
[SugarTable(TableName = "base_pathology_sample_type")]
public partial class BasePathologySampleTypeEntity : EntityBase
{
    /// <summary>
    /// 标本类型代码
    /// </summary>
    /// <remarks>标本类型代码</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", ColumnDataType = "varchar", Length = 16)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 标本类型名称
    /// </summary>
    /// <remarks>标本类型名称</remarks>
    [SugarColumn(ColumnName = "SampleTypeName", ColumnDataType = "varchar", Length = 32)]
    public string? SampleTypeName { get; set; }
    /// <summary>
    /// 父级代码
    /// </summary>
    /// <remarks>父级代码</remarks>
    [SugarColumn(ColumnName = "ParentCode", ColumnDataType = "varchar", Length = 16)]
    public string? ParentCode { get; set; }
    /// <summary>
    /// 级别
    /// </summary>
    /// <remarks>级别</remarks>
    [SugarColumn(ColumnName = "TypeGrade", ColumnDataType = "int", DecimalDigits = 2)]
    public int? TypeGrade { get; set; }
    /// <summary>
    /// 疾病代码
    /// </summary>
    /// <remarks>疾病代码</remarks>
    [SugarColumn(ColumnName = "DiseaseCode", ColumnDataType = "varchar", Length = 256)]
    public string? DiseaseCode { get; set; }
    /// <summary>
    /// 模板代码
    /// </summary>
    /// <remarks>模板代码</remarks>
    [SugarColumn(ColumnName = "TemplateCode", ColumnDataType = "varchar", Length = 256)]
    public string? TemplateCode { get; set; }
    /// <summary>
    /// 拼音
    /// </summary>
    /// <remarks>拼音</remarks>
    [SugarColumn(ColumnName = "PinYin", ColumnDataType = "varchar", Length = 16)]
    public string? PinYin { get; set; }
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
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

