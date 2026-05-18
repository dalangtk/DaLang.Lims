namespace DaLang.Lims.Pathology.Contracts.PathologyTest.Dto;

public class PathologyDoctor
{
    public long? FirstDoctorId { get; set; }
    public string? FirstDoctor { get; set; }
    public long? SecondDoctorId { get; set; }
    public string? SecondDoctor { get; set; }
    public long? ReportDoctorId { get; set; }
    public string? ReportDoctor { get; set; }
    public DateTime? ReportTime { get; set; }
}
