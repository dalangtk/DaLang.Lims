namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

/// <summary>
/// 目的机构设置分页查询输出
/// </summary>
public partial class BasePurposeTenantSettingGetListDto : BasePurposeTenantSettingDto
{
    /// <summary>
    /// 创建者Id
    /// </summary>
    public long? ProId { get; set; }
    /// <summary>
    /// 创建者姓名
    /// </summary>
    public string? ProName { get; set; }
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime ProTime { get; set; }
    /// <summary>
    /// 修改者Id
    /// </summary>
    public long? ModId { get; set; }
    /// <summary>
    /// 修改者姓名
    /// </summary>
    public string? ModName { get; set; }
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? ModTime { get; set; }
}
