namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

public class SavePurposeInput
{
    public BasePurposeDto Purpose { get; set; }
    public List<BasePurposeDetailDto>? PurposeItemDetail { get; set; }
    public List<BasePurposePersonalizeDto>? PurposePersonalizes { get; set; }
    public BasePurposeTenantSettingDto PurposeTenantSetting { get; set; }
}
