namespace DaLang.Lims.Statistics.Contracts.SampleQuery.Dto;

public class SampleQueryInput
{
    public string? PatientName { get; set; }
    public string CustomerCode { get; set; }
    public string? Barcode { get; set; }
    public DateTime? BeginTime { get; set; }
    public DateTime? EndTime { get; set; }
    public List<string> PurCodes { get; set; } = new List<string>();
}