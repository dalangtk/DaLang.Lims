namespace DaLang.Lims.BaseData.Contracts.AuditRule.Dto;

public class TestRuleResultDto
{
    public List<TestRuleDto> TriggerRules { get; set; } = new List<TestRuleDto>();
    public List<string>? IgnoreRules { get; set; }
}
public class TestRuleDto
{
    public bool IsTrigger { get; set; }
    /// <summary>
    ///规则代码
    ///</summary>
    public string RuleCode { get; set; }
    /// <summary>
    ///规则名称
    ///</summary>
    public string RuleName { get; set; }
    /// <summary>
    ///提示类型 提示/警告/禁止
    ///</summary>
    public int NoticeType { get; set; }
    /// <summary>
    ///规则等级
    ///</summary>
    public int RuleProperty { get; set; }
    /// <summary>
    /// 提示消息
    /// </summary>
    public string NoticeMessage { get; set; }
    /// <summary>
    /// 异常
    /// </summary>
    public bool IsException { get; set; } = false;
}
