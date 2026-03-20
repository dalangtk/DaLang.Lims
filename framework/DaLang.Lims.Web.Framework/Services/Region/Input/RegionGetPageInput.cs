using DaLang.Lims.Web.Framework.Domain.Region;

namespace DaLang.Lims.Web.Framework.Services.Region;

public class RegionGetPageInput
{
    /// <summary>
    /// 上级Id
    /// </summary>
    public long? ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    public RegionLevel? Level { get; set; }

    /// <summary>
    /// 热门
    /// </summary>
    public bool? Hot { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool? IsValid { get; set; }


}