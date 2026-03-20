namespace DaLang.Lims.Pretreatment.Contracts.SampleInput.Dto;

public class InputItemDto
{
    public string PurCode { get; set; }
    public string PurName { get; set; }
    public string SampleTypeCode { get; set; }
    public bool IsCombo { get; set; } = false;
    public string? ComboCode { get; set; }
    public string? ComboName { get; set; }
}
