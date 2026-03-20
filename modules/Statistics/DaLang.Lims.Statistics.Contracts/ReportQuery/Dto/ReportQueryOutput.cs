using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;
using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;

namespace DaLang.Lims.Statistics.Contracts.ReportQuery.Dto;

public class ReportQueryDto : ExamInfoDto
{
    public string? ReportUrl { get; set; }
    public string? InfoPurCodes { get; set; }
    public string? InfoPurNames { get; set; }
    public List<ApplyPurposeDto> PurposeList { get; set; } = new List<ApplyPurposeDto>();
}
public class ReportQueryOutput : ExamInfoDto
{
    public string? ReportUrl { get; set; }
}
