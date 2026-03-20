namespace DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;

/// <summary>
/// 分拣信息
/// </summary>
public class PretreatSortInfoDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///分拣代码
    ///</summary>
    public long? SortInfoCode { get; set; }
    /// <summary>
    ///分拣仪代码
    ///</summary>
    public string? SorterCode { get; set; }
    /// <summary>
    ///分拣开始时间
    ///</summary>
    public DateTime? StartTime { get; set; }
    /// <summary>
    ///分拣结束时间
    ///</summary>
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 上架模式0否 1是
    /// </summary>
    public int? IsShelfMode { get; set; } = 0;
    /// <summary>
    ///分拣状态 0已结束
    ///</summary>
    public int? Status { get; set; }
    /// <summary>
    ///主机名
    ///</summary>
    public string? HostName { get; set; }
    /// <summary>
    ///创建者Id
    ///</summary>
    public long? ProId { get; set; }
    /// <summary>
    ///创建者姓名
    ///</summary>
    public string? ProName { get; set; }
    /// <summary>
    ///创建时间
    ///</summary>
    public DateTime ProTime { get; set; }
    /// <summary>
    ///修改者Id
    ///</summary>
    public long? ModId { get; set; }
    /// <summary>
    ///修改者姓名
    ///</summary>
    public string? ModName { get; set; }
    /// <summary>
    ///更新时间
    ///</summary>
    public DateTime? ModTime { get; set; }
}
