using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;

namespace DaLang.Lims.Exam.Contracts.SampleTest.Dto;

public class UpdatePatientInfoInput
{
    public ExamInfoUpdateInput ExamInfo { get; set; }
    public List<string> UpdateFields { get; set; }
}
