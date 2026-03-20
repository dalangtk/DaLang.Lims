namespace DaLang.Lims.BaseData.Contracts.Group.Dto;
#pragma warning disable CS8618
/// <summary>
/// 组别
/// </summary>
public class BaseGroupDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    /// 组别代码
    /// </summary>
    public string GroupCode { get; set; }
    /// <summary>
    /// 组别名称
    /// </summary>
    public string GroupName { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; }
}
#pragma warning restore CS8618
