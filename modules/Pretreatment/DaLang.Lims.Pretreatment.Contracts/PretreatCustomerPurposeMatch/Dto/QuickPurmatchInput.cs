namespace DaLang.Lims.Pretreatment.Contracts.PretreatCustomerPurposeMatch.Dto;

public class QuickPurmatchInput : PurposeMatchMainDto
{
    public List<PurMatchCentralPurposeDto> CentralPurList { get; set; } = new List<PurMatchCentralPurposeDto>();
}
public class PurMatchCentralPurposeDto
{
    public string CentralPurCode { get; set; }
    public bool IsCombo { get; set; }
}
