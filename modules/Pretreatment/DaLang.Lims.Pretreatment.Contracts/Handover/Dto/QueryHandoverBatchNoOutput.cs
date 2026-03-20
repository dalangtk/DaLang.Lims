namespace DaLang.Lims.Pretreatment.Contracts.Handover.Dto;

public class QueryHandoverBatchNoOutput
{
    public long BatchNo { get; set; }
    public long HandoverId { get; set; }
    public string HandoverName { get; set; }
    public DateTime HandoverTime { get; set; }
    public int Total { get; set; }
}
