namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

public class QueryPurposeAndComboInput
{
    public string? CustomerCode { get; set; }
    public string? PurCode { get; set; }
    public bool QueryCombo { get; set; } = true;
    public string? GroupCode { get; set; }
}
