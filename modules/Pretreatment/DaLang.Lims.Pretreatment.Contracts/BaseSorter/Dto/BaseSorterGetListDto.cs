namespace DaLang.Lims.Pretreatment.Contracts.BaseSorter.Dto;

///<summary>
///分拣仪器分页查询结果输出
///</summary>
public partial class BaseSorterGetListDto : BaseSorterDto
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
