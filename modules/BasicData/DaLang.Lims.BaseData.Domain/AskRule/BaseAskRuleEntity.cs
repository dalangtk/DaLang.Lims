using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.BaseAskRule;

/// <summary>
/// 问询规则 实体类
/// </summary>
/// <remarks>问询规则</remarks>
[SugarTable(TableName = "base_ask_rule")]
public partial class BaseAskRuleEntity : EntityTenant
{
    /// <summary>
    /// 问询规则代码
    /// </summary>
    /// <remarks>问询规则代码</remarks>
    [SugarColumn(ColumnName = "AskRuleCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string AskRuleCode { get; set; }
    /// <summary>
    /// 问询规则名称
    /// </summary>
    /// <remarks>问询规则名称</remarks>
    [SugarColumn(ColumnName = "AskRuleName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string AskRuleName { get; set; }
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

