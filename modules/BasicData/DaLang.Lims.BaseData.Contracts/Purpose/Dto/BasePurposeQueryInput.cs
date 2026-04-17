namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

public class BasePurposeQueryInput
{
    public string? GroupCode { get; set; }
    public string? PurCode { get; set; }
    public bool? ContainsPathology { get; set; } = false;
}
