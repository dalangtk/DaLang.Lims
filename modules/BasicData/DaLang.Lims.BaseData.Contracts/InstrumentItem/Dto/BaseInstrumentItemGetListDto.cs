namespace DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;

#pragma warning disable CS8618
/// <summary>上机项目
///分页查询结果输出
///</summary>
public partial class BaseInstrumentItemGetListDto : BaseInstrumentItemDto
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
    /// <summary>
    ///已修改
    ///</summary>
    public bool IsModified { get; set; }
    /// <summary>
    ///已删除
    ///</summary>
    public bool IsDeleted { get; set; }
}
#pragma warning restore CS8618