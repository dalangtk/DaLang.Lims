namespace DaLang.Lims.Shared.Contracts.ExamSampleTrack.Dto;

/// <summary>
/// 样本跟踪查询列表输出
/// </summary>
public partial class ExamSampleTrackGetListDto : ExamSampleTrackDto
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
