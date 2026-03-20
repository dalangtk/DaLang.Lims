using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.BaseData.Contracts.TenantReportExtend.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamResult.Dto;

namespace DaLang.Lims.ReportTemplate.Contracts.ReportTemplate.Dto;

public class ExamReportSourceDto : ExamInfoDto
{
    public List<ExamResultDto> ResultList { get; set; }
    public ReportExtendDto ReportExtend { get; set; }
}
