using DaLang.Lims.Shared.Contracts.ExamSpecialResult.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologyTest.Dto;

public class SaveResultInput
{
    public long ExamInfoId { get; set; }
    public int ResultType { get; set; }
    public List<ExamSpecialResultDto> SpecialResultList { get; set; } = new List<ExamSpecialResultDto>();
    public PathologyDoctor? Doctor { get; set; }
}
