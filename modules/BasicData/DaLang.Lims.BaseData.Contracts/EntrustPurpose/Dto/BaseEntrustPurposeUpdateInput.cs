namespace DaLang.Lims.BaseData.Contracts.EntrustPurpose.Dto;

/// <summary>
/// 委托目的更新数据输入
/// </summary>
public partial class BaseEntrustPurposeUpdateInput : BaseEntrustPurposeAddInput
{
    public long Id { get; set; }
}
