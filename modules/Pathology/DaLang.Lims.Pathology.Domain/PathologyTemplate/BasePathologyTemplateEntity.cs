using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.Pathology.Domain.PathologyTemplate;

/// <summary>
/// 诊断模板 实体类
/// </summary>
/// <remarks>病理诊断模板</remarks>
[SugarTable(TableName = "base_pathology_template")]
public partial class BasePathologyTemplateEntity : EntityTenant
{
    /// <summary>
    /// 工作流
    /// </summary>
    /// <remarks>工作流</remarks>
    [SugarColumn(ColumnName = "WFCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? WFCode { get; set; }
    /// <summary>
    /// 模板代码
    /// </summary>
    /// <remarks>模板代码</remarks>
    [SugarColumn(ColumnName = "TemplateCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? TemplateCode { get; set; }
    /// <summary>
    /// 模板名称
    /// </summary>
    /// <remarks>模板名称</remarks>
    [SugarColumn(ColumnName = "TemplateName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? TemplateName { get; set; }
    /// <summary>
    /// 模板内容
    /// </summary>
    /// <remarks>模板内容</remarks>
    [SugarColumn(ColumnName = "TemplateContent", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "text", Length = 0)]
    public string? TemplateContent { get; set; }
    /// <summary>
    /// 诊断意见
    /// </summary>
    /// <remarks>诊断意见</remarks>
    [SugarColumn(ColumnName = "DiagnosisResult", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? DiagnosisResult { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCodes", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? CustomerCodes { get; set; }
    /// <summary>
    /// 客户类型
    /// </summary>
    /// <remarks>客户类型</remarks>
    [SugarColumn(ColumnName = "CustomerType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? CustomerType { get; set; }
    /// <summary>
    /// 模板类型 1诊断 2巨检
    /// </summary>
    /// <remarks>模板类型</remarks>
    [SugarColumn(ColumnName = "TemplateType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int? TemplateType { get; set; }
    /// <summary>
    /// 默认结果
    /// </summary>
    /// <remarks>默认结果</remarks>
    [SugarColumn(ColumnName = "IsDefaultResult", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool? IsDefaultResult { get; set; }
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
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}
