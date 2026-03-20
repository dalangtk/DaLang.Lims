using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.AuditRule;

/// <summary>
/// 审核规则 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "base_audit_rule")]
public partial class BaseAuditRuleEntity : EntityTenant
{
    /// <summary>
    /// 规则代码
    /// </summary>
    /// <remarks>规则代码</remarks>
    [SugarColumn(ColumnName = "RuleCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string RuleCode { get; set; }
    /// <summary>
    /// 规则名称
    /// </summary>
    /// <remarks>规则名称</remarks>
    [SugarColumn(ColumnName = "RuleName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string RuleName { get; set; }
    /// <summary>
    /// 规则描述
    /// </summary>
    /// <remarks>规则描述</remarks>
    [SugarColumn(ColumnName = "RuleDescription", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string RuleDescription { get; set; }
    /// <summary>
    /// 规则内容
    /// </summary>
    /// <remarks>规则内容</remarks>
    [SugarColumn(ColumnName = "RuleExpression", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 512)]
    public string RuleExpression { get; set; }
    /// <summary>
    /// 组别
    /// </summary>
    /// <remarks>组别</remarks>
    [SugarColumn(ColumnName = "GroupCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? GroupCode { get; set; }
    /// <summary>
    /// 判断类型 项目/信息
    /// </summary>
    /// <remarks>判断类型 项目/信息</remarks>
    [SugarColumn(ColumnName = "JudgeType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int JudgeType { get; set; }
    /// <summary>
    /// 工作流
    /// </summary>
    /// <remarks>工作流</remarks>
    [SugarColumn(ColumnName = "WorkFlowType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? WorkFlowType { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCodes", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 255)]
    public string? PurCodes { get; set; }
    /// <summary>
    /// 项目代码
    /// </summary>
    /// <remarks>项目代码</remarks>
    [SugarColumn(ColumnName = "ItemCodes", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 255)]
    public string? ItemCodes { get; set; }
    /// <summary>
    /// 完全包含
    /// </summary>
    /// <remarks>完全包含</remarks>
    [SugarColumn(ColumnName = "IsAllContains", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "false")]
    public bool IsAllContains { get; set; } = false;
    /// <summary>
    /// 提示类型 提示/警告/禁止
    /// </summary>
    /// <remarks>提示类型 提示/警告/禁止</remarks>
    [SugarColumn(ColumnName = "NoticeType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int NoticeType { get; set; }
    /// <summary>
    /// 规则等级
    /// </summary>
    /// <remarks>规则等级</remarks>
    [SugarColumn(ColumnName = "RuleProperty", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, DefaultValue = "0")]
    public int RuleProperty { get; set; }
    /// <summary>
    /// 审核类型 批量或单个
    /// </summary>
    /// <remarks>审核类型 批量或单个</remarks>
    [SugarColumn(ColumnName = "AuditType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, DefaultValue = "0")]
    public int AuditType { get; set; }
    /// <summary>
    /// 提示消息
    /// </summary>
    /// <remarks>提示消息</remarks>
    [SugarColumn(ColumnName = "NoticeMessage", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? NoticeMessage { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
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

#pragma warning restore CS8618

