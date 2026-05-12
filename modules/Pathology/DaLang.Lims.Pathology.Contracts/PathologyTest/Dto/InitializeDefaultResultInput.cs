using DaLang.Lims.Exam.Contracts.Pathology.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologyTest.Dto;

public class InitializeDefaultResultInput : PathologyReceiveInput
{
    public long ExamInfoId { get; set; }
    public int ResultType { get; set; }
    public DateTime TestDate { get; set; }
}
