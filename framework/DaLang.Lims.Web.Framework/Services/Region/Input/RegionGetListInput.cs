namespace DaLang.Lims.Web.Framework.Services.Region;

public class RegionGetListInput
{
    /// <summary>
    /// 上级Id
    /// </summary>
    public long ParentId { get; set; }

    /// <summary>
    /// 热门
    /// </summary>
    public bool? Hot { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool? IsValid { get; set; }
}