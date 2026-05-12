using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.Pathology.Domain.PathologySetting;

/// <summary>
/// 病理配置 实体类
/// </summary>
/// <remarks>病理设置表</remarks>
[SugarTable(TableName = "base_pathology_setting")]
public partial class BasePathologySettingEntity : EntityTenant
{
    /// <summary>
    /// 工作流
    /// </summary>
    /// <remarks>工作流</remarks>
    [SugarColumn(ColumnName = "WFCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? WFCode { get; set; }
    /// <summary>
    /// 审核模式 1初复诊 2复诊
    /// </summary>
    /// <remarks>审核模式 1初复诊 2复诊</remarks>
    [SugarColumn(ColumnName = "AuditType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, DefaultValue = "2")]
    public int AuditType { get; set; } = 2;
    /// <summary>
    /// 前缀
    /// </summary>
    /// <remarks>前缀</remarks>
    [SugarColumn(ColumnName = "SampleNoSymbol", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? SampleNoSymbol { get; set; }
    /// <summary>
    /// 审核人Id
    /// </summary>
    /// <remarks>审核人Id</remarks>
    [SugarColumn(ColumnName = "ReviewUserId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ReviewUserId { get; set; }
    /// <summary>
    /// 审核人
    /// </summary>
    /// <remarks>审核人</remarks>
    [SugarColumn(ColumnName = "ReviewUserName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ReviewUserName { get; set; }
    /// <summary>
    /// 报告周期
    /// </summary>
    /// <remarks>报告周期</remarks>
    [SugarColumn(ColumnName = "ReportCycle", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int? ReportCycle { get; set; }
    /// <summary>
    /// 允许单医生
    /// </summary>
    /// <remarks>允许单医生</remarks>
    [SugarColumn(ColumnName = "CanSameUserReport", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "false")]
    public bool CanSameUserReport { get; set; } = false;
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
