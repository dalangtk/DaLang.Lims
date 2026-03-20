namespace DaLang.Lims.Shared.Contracts.ApplyItem.Dto;

/// <summary>
/// 申请单项目分页查询条件输入
/// </summary>
public partial class ApplyItemQueryInput
{
    public string Barcode { get; set; }
    public string? PurCode { get; set; }
}
