namespace DaLang.Lims.BaseData.Contracts.Item.Dto;

#pragma warning disable CS8618
/// <summary>
/// 基础项目
/// </summary>
public class BaseItemDto
{
    /// <summary>
    /// Id
    /// </summary>
    public virtual long Id { get; set; }
    /// <summary>
    /// 组别代码
    /// </summary>
    public string GroupCode { get; set; }
    /// <summary>
    /// 组别名称
    /// </summary>
    public string GroupName { get; set; }
    /// <summary>
    /// 项目代码
    /// </summary>
    public string? ItemCode { get; set; }
    /// <summary>
    /// 项目名称
    /// </summary>
    public string ItemName { get; set; }
    /// <summary>
    /// 结果类型，定量、定性等
    /// </summary>
    public string? ResultType { get; set; }
    /// <summary>
    /// 结果判定方式，高低/阴阳性/不判定
    /// </summary>
    public string? DecideType { get; set; }
    /// <summary>
    /// 缩写
    /// </summary>
    public string? ItemNameAB { get; set; }
    /// <summary>
    /// 英文
    /// </summary>
    public string? ItemNameEN { get; set; }
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