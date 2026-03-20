namespace DaLang.Lims.BaseData.Contracts.Combo.Dto;

public class ComboAddOrUpdateInput
{
    public BaseComboUpdateInput Combo { get; set; }
    public List<string> ComboDetail { get; set; }
}
