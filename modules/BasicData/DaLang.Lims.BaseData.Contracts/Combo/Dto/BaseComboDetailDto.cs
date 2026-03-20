namespace DaLang.Lims.BaseData.Contracts.Combo.Dto;

public class BaseComboDetailDto : BaseComboDetailUpdateInput
{
    public string ComboCode { get; set; }
    public string ComboName { get; set; }
    public string GroupName { get; set; }
    public string SampleTypeCode { get; set; }
    public string SampleTypeName { get; set; }
}
