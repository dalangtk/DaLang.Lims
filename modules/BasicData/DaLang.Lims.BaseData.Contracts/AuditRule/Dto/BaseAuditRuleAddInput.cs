namespace DaLang.Lims.BaseData.Contracts.AuditRule.Dto;

public class BaseAuditRuleAddInput
{
    /// <summary>
    ///规则代码
    ///</summary>
    public string RuleCode { get; set; }
    /// <summary>
    ///规则名称
    ///</summary>
    public string RuleName { get; set; }
    /// <summary>
    ///规则描述
    ///</summary>
    public string RuleDescription { get; set; }
    /// <summary>
    ///规则内容
    ///</summary>
    public string RuleExpression { get; set; }
    /// <summary>
    ///组别
    ///</summary>
    public string? GroupCode { get; set; }
    /// <summary>
    ///判断类型 项目/信息
    ///</summary>
    public int JudgeType { get; set; }
    /// <summary>
    ///工作流
    ///</summary>
    public string? WorkFlowType { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string? PurCodes { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string? ItemCodes { get; set; }
    /// <summary>
    /// 完全包含
    /// </summary>
    public bool IsAllContains { get; set; } = false;
    /// <summary>
    ///提示类型 提示/警告/禁止
    ///</summary>
    public int NoticeType { get; set; }
    /// <summary>
    ///规则等级
    ///</summary>
    public int RuleProperty { get; set; }
    /// <summary>
    /// 审核类型 批量或单个
    /// </summary>
    public int AuditType { get; set; }
    /// <summary>
    /// 提示消息
    /// </summary>
    public string NoticeMessage { get; set; }
    /// <summary>
    ///备注
    ///</summary>
    public string? Remark { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
