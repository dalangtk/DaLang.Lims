namespace DaLang.Lims.Client.Handler.Dto;

public class SnapShotDto
{
    public string Component { get; set; }
    public string SampleNo { get; set; }
    public string AccessToken { get; set; }
    public long ExamInfoId { get; set; }
    public bool IsGrossExamination { get; set; } = false;
}
