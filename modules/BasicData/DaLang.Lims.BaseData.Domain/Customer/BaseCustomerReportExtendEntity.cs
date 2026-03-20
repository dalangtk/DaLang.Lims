using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.Customer;

/// <summary>
/// 客户报告扩展 实体类
/// </summary>
/// <remarks>客户报告扩展</remarks>
[SugarTable(TableName = "base_customer_report_extend")]
public partial class BaseCustomerReportExtendEntity : EntityTenant
{
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// logo
    /// </summary>
    /// <remarks>logo</remarks>
    [SugarColumn(ColumnName = "CustomerLogo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? CustomerLogo { get; set; }
    /// <summary>
    /// 客户签章
    /// </summary>
    /// <remarks>客户签章</remarks>
    [SugarColumn(ColumnName = "CustomerSignature", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? CustomerSignature { get; set; }
    /// <summary>
    /// 是否生成报告
    /// </summary>
    /// <remarks>是否生成报告</remarks>
    [SugarColumn(ColumnName = "IsGenerateReport", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool? IsGenerateReport { get; set; }
    /// <summary>
    /// 报告优先级
    /// </summary>
    /// <remarks>报告优先级</remarks>
    [SugarColumn(ColumnName = "ReportPriority", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int? ReportPriority { get; set; }
    /// <summary>
    /// 图片DPI
    /// </summary>
    /// <remarks>图片DPI</remarks>
    [SugarColumn(ColumnName = "ImageDpi", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int? ImageDpi { get; set; }
    /// <summary>
    /// 报告语言
    /// </summary>
    /// <remarks>报告语言</remarks>
    [SugarColumn(ColumnName = "LanguageType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? LanguageType { get; set; }
    /// <summary>
    /// 报告主标题
    /// </summary>
    /// <remarks>报告主标题</remarks>
    [SugarColumn(ColumnName = "ReportMainTitle", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? ReportMainTitle { get; set; }
    /// <summary>
    /// 报告子标题
    /// </summary>
    /// <remarks>报告子标题</remarks>
    [SugarColumn(ColumnName = "ReportSubTitle", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? ReportSubTitle { get; set; }
    /// <summary>
    /// 地址
    /// </summary>
    /// <remarks>地址</remarks>
    [SugarColumn(ColumnName = "Address", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 255)]
    public string? Address { get; set; }
    /// <summary>
    /// 电话
    /// </summary>
    /// <remarks>电话</remarks>
    [SugarColumn(ColumnName = "Telephone", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? Telephone { get; set; }
    /// <summary>
    /// 生成抬头类型 0中心 1客户
    /// </summary>
    /// <remarks>生成抬头类型</remarks>
    [SugarColumn(ColumnName = "GenerateTitleType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int? GenerateTitleType { get; set; }
    /// <summary>
    /// 允许查看报告途径
    /// </summary>
    /// <remarks>允许查看报告途径</remarks>
    [SugarColumn(ColumnName = "CanSearchReportChannel", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? CanSearchReportChannel { get; set; }
    /// <summary>
    /// 纸张类型 0 A5 1 A4
    /// </summary>
    /// <remarks>纸张类型</remarks>
    [SugarColumn(ColumnName = "PaperType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int PaperType { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "1")]
    public bool IsValid { get; set; } = true;

}

#pragma warning restore CS8618

