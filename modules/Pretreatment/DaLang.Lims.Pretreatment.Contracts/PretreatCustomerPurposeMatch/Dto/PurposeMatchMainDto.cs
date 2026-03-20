namespace DaLang.Lims.Pretreatment.Contracts.PretreatCustomerPurposeMatch.Dto;

public class PurposeMatchMainDto
{
    public string CustomerCode { get; set; }
    public string CustomerPurCode { get; set; }
    public string CustomerPurName { get; set; }
}
public class PurposeMatchDetailDto : PurposeMatchMainDto
{
    public string CentralPurCode { get; set; }
    public string CentralPurName { get; set; }
}
