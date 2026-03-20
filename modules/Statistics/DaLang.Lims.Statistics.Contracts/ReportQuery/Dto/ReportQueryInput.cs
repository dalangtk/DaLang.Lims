namespace DaLang.Lims.Statistics.Contracts.ReportQuery.Dto;

public class ReportQueryInput
{
    public string? PatientName { get; set; }
    public string? Customer { get; set; }
    public string? Barcode { get; set; }
    public DateTime? Begin { get; set; }
    public DateTime? End { get; set; }
    public string? LogisticsRoute { get; set; }
    public string? PurCodes { get; set; }
    public TimeTypeEnum TimeType { get; set; }
}
public enum TimeTypeEnum
{
    ReportTime = 0,
    ReceiveTime = 1,
    TestTime = 2
}
