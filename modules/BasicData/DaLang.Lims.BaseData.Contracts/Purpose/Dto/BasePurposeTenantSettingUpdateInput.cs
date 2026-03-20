namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

/// <summary>
/// 目的机构设置更新输入
/// </summary>
public partial class BasePurposeTenantSettingUpdateInput : BasePurposeTenantSettingAddInput
{
    public long Id { get; set; }
}
