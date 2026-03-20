using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.BaseData.Domain.TenantReportExtend;

/// <summary>
/// 机构设置 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "base_tenant_report_extend")]
public partial class BaseTenantReportExtendEntity : EntityTenant
{
    /// <summary>
    /// logo
    /// </summary>
    /// <remarks>logo</remarks>
    [SugarColumn(ColumnName = "ReportLogo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? ReportLogo { get; set; }
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
    /// 签章
    /// </summary>
    /// <remarks>签章</remarks>
    [SugarColumn(ColumnName = "ReportSignature", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? ReportSignature { get; set; }
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
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}
