using DaLang.Lims.Web.Framework.Core.Attributes;
using System.ComponentModel;

namespace DaLang.Lims.Pathology.Core.Consts;

public static partial class PathologyConsts
{
    /// <summary>
    /// 默认域
    /// </summary>
    public const string AreaName = "pathology";
    public static string[] OtherResultField = ["internalNote"];
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
public static partial class PathologyCacheKeys
{
    public const string PathologySettingKey = "pathology:settings:";
}