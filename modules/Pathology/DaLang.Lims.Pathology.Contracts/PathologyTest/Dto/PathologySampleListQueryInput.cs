using DaLang.Lims.Shared.Contracts.ExamInfo.Dto;

namespace DaLang.Lims.Exam.Contracts.Pathology.Dto;

public class PathologySampleListQueryInput : ExamListQueryInput
{
    public string? WFCode { get; set; }
    public bool OnlySelf { get; set; } = true;
}
