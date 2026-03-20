using DaLang.Lims.Pretreatment.Core.Enum;

namespace DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;

/// <summary>
/// 分拣信息详情
/// </summary>
public class PretreatSortDetailDto
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
    ///架子位置
    ///</summary>
    public int? ShelfPosition { get; set; }
    /// <summary>
    ///架子代码
    ///</summary>
    public string? ShelfCode { get; set; }
    /// <summary>
    ///架子名称
    ///</summary>
    public string? ShelfName { get; set; }
    /// <summary>
    ///架子类型 0常规1分血
    ///</summary>
    public PretreatShelfTypeEnum ShelfType { get; set; }
    /// <summary>
    ///行孔数
    ///</summary>
    public int? RowHoleCount { get; set; }
    /// <summary>
    ///列孔数
    ///</summary>
    public int? ColumnHoleCount { get; set; }
    /// <summary>
    ///使用数
    ///</summary>
    public int? UsedCount { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int? Sort { get; set; }
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
