namespace DaLang.Lims.BaseData.Contracts.Combo.Dto;

public class BaseComboWithPurcodesDto : BaseComboDto
{
    public List<string> PurposeList { get; set; } = new List<string>();
}
