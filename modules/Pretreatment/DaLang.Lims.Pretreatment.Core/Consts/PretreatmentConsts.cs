using DaLang.Lims.Web.Framework.Core.Attributes;
using System.ComponentModel;

namespace DaLang.Lims.Pretreatment.Core.Consts;

public static partial class PretreatmentConsts
{
    /// <summary>
    /// 默认域
    /// </summary>
    public const string AreaName = "pretreatment";

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
public static class PretreatmentCacheKeys
{
    public static string SortInfoCache = "sortinfo:";
}
