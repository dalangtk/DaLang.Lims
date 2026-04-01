namespace DaLang.Lims.Api.Contracts.Dto
{
    public class EntrustResultQueryInpupt
    {
        public string? Barcode { get; set; }
        public string? CustomerCode { get; set; }
        public EntrustResultQueryTimeType TimeType { get; set; }
        public DateTime? Begin { get; set; }
        public DateTime? End { get; set; }
    }
    public enum EntrustResultQueryTimeType
    {
        ReceiveTime = 1,
        ReportTime = 2
    }
}
