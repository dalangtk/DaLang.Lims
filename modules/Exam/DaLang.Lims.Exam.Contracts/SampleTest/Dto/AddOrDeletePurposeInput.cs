namespace DaLang.Lims.Exam.Contracts.SampleTest.Dto;

public class AddOrDeletePurposeInput
{
    public long ExamInfoId { get; set; }
    public List<string>? ComboCodes { get; set; }
    public List<string>? PurCodes { get; set; }
}
