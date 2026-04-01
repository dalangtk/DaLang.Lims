using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;
using DaLang.Lims.Shared.Contracts.ExamResult.Dto;

namespace DaLang.Lims.Api.Contracts.Dto;

public class EntrustResultDto
{
    public ExamInfoDto ExamInfo { get; set; }
    public List<ExamResultDto> ResultList { get; set; }
}
