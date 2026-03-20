using DaLang.Lims.BaseData.Contracts.AuditRule.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;

namespace DaLang.Lims.Exam.Contracts.SampleTest.Dto;

public class AuditResultDto
{
    public ExamInfoDto ExamInfo { get; set; }
    public List<TestRuleDto> TriggerRules { get; set; } = new List<TestRuleDto>();
}
