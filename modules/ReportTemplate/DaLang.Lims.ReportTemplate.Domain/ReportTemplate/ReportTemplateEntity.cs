using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.ReportTemplate.Domain.ReportTemplate;

/// <summary>
/// 模板 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "report_template")]
public partial class ReportTemplateEntity : EntityTenant
{
    /// <summary>
    /// 模板代码
    /// </summary>
    /// <remarks>模板代码</remarks>
    [SugarColumn(ColumnName = "TemplateCode", ColumnDataType = "varchar", Length = 32)]
    public string TemplateCode { get; set; }
    /// <summary>
    /// 模板名称
    /// </summary>
    /// <remarks>模板名称</remarks>
    [SugarColumn(ColumnName = "TemplateName", ColumnDataType = "varchar", Length = 64)]
    public string TemplateName { get; set; }
    /// <summary>
    /// 描述
    /// </summary>
    /// <remarks>描述</remarks>
    [SugarColumn(ColumnName = "TemplateDescription", ColumnDataType = "varchar", Length = 128)]
    public string? TemplateDescription { get; set; }
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", ColumnDataType = "varchar", Length = 16)]
    public string? GroupCode { get; set; }
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "HospitalCode", ColumnDataType = "varchar", Length = 16)]
    public string? HospitalCode { get; set; }
    /// <summary>
    /// 项目代码
    /// </summary>
    /// <remarks>项目代码</remarks>
    [SugarColumn(ColumnName = "ItemCodes", ColumnDataType = "varchar", Length = 128)]
    public string? ItemCodes { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCodes", ColumnDataType = "varchar", Length = 128)]
    public string? PurCodes { get; set; }
    /// <summary>
    /// 样本号
    /// </summary>
    /// <remarks>样本号</remarks>
    [SugarColumn(ColumnName = "SampleNoSymbols", ColumnDataType = "varchar", Length = 128)]
    public string? SampleNoSymbols { get; set; }
    /// <summary>
    /// 性别代码
    /// </summary>
    /// <remarks>性别代码</remarks>
    [SugarColumn(ColumnName = "GenderCode", ColumnDataType = "varchar", Length = 8)]
    public string? GenderCode { get; set; }
    /// <summary>
    /// 起始时间
    /// </summary>
    /// <remarks>起始时间</remarks>
    [SugarColumn(ColumnName = "BeginTime", ColumnDataType = "datetime")]
    public DateTime? BeginTime { get; set; }
    /// <summary>
    /// 截止时间
    /// </summary>
    /// <remarks>截止时间</remarks>
    [SugarColumn(ColumnName = "EndTime", ColumnDataType = "datetime")]
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 子报表
    /// </summary>
    /// <remarks>子报表</remarks>
    [SugarColumn(ColumnName = "IsSubTemplate", ColumnDataType = "tinyint")]
    public bool? IsSubTemplate { get; set; }
    /// <summary>
    /// 模板类型
    /// </summary>
    /// <remarks>模板类型</remarks>
    [SugarColumn(ColumnName = "TemplateType", ColumnDataType = "int", DecimalDigits = 2)]
    public int? TemplateType { get; set; }
    /// <summary>
    /// 模板内容
    /// </summary>
    /// <remarks>模板内容</remarks>
    [SugarColumn(ColumnName = "TemplateContent", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "text", Length = 0)]
    public string? TemplateContent { get; set; }
    /// <summary>
    /// 数据源
    /// </summary>
    /// <remarks>数据源</remarks>
    [SugarColumn(ColumnName = "TemplateDataSource", ColumnDataType = "varchar", Length = 32)]
    public string? TemplateDataSource { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

