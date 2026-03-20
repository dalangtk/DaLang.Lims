namespace DaLang.Lims.Pretreatment.Contracts.Entrust.Dto;

public class CalcEntrustInput
{
    public string CustomerCode { get; set; }
    public List<string> PurCodeList { get; set; } = new List<string>();
    public string SampleTypeCode { get; set; }
    public DateTime ReceiveTime { get; set; }
    public bool ThrowNotExists { get; set; } = false;
}
public class CalcEntrustDto
{
    public string PurCode { get; set; }
    public bool IsEntrust { get; set; } = false;
    public string EntrustHospitalCode { get; set; }
    public string EntrustHospitalName { get; set; }
    public DateTime? EstimatedAskTime { get; set; }
}
