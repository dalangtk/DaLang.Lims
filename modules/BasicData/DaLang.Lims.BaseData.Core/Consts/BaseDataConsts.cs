using DaLang.Lims.Web.Framework.Core.Attributes;
using System.ComponentModel;

namespace DaLang.Lims.Web.BaseData.Core.Consts;

public static partial class BaseDataConsts
{
    /// <summary>
    /// 默认域
    /// </summary>
    public const string AreaName = "lims";

}

/// <summary>
/// 数据库键名
/// </summary>
public class DbKeys
{
    /// <summary>
    /// 数据库注册键
    /// </summary>
    [Description("数据库注册键")]
    public static string AppDb { get; set; } = "admindb";

}

/// <summary>
/// 缓存键
/// </summary>
[ScanCacheKeys]
public static class BaseDataCacheKeys
{
    /// <summary>
    /// 标本类型缓存 dalang:basedata:sampletype:
    /// </summary>
    [Description("标本类型缓存")]
    public const string SampleType = "dalang:basedata:sampletype:";
}