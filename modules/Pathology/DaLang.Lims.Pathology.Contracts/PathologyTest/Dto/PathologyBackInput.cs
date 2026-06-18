namespace DaLang.Lims.Pathology.Contracts.PathologyTest.Dto;

public class PathologyBackInput
{
    public string WFCode { get; set; }
    public List<long> ExamInfoIdList { get; set; } = new List<long>();
}
