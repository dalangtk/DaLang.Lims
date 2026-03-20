using DaLang.Lims.Web.Common.Enums;

namespace DaLang.Lims.BaseData.Contracts.AuditRule.Dto;

public class TestRuleInput
{
    public long ExamInfoId { get; set; }
    public string? RuleCode { get; set; }
    public List<string>? IgnoreRuleCodes { get; set; }
    public bool ShowDetail { get; set; } = false;
    public ExecuteTypeEnum ExecuteType { get; set; } = ExecuteTypeEnum.Single;
}
