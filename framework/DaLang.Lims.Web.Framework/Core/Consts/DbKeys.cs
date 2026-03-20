using System.ComponentModel;

namespace DaLang.Lims.Web.Framework.Core.Consts;

/// <summary>
/// 数据库键名
/// </summary>
public class DbKeys
{
    /// <summary>
    /// 默认主数据库标识（默认租户）
    /// </summary>
    public const string MainConfigId = "maindb";

    /// <summary>
    /// 默认日志数据库标识
    /// </summary>
    public const string LogConfigId = "maindb";

    /// <summary>
    /// 默认表主键
    /// </summary>
    public const string PrimaryKey = "Id";

    /// <summary>
    /// 默认租户Id
    /// </summary>
    public const long DefaultTenantId = 1001;
}