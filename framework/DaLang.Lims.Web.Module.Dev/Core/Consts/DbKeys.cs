using System.ComponentModel;

namespace DaLang.Lims.Web.Dev.Core.Consts;

/// <summary>
/// 数据库键名
/// </summary>
public class DbKeys
{
    /// <summary>
    /// 数据库注册键
    /// </summary>
    [Description("数据库注册键")]
    public static string AppDb { get; set; } = Framework.Core.Consts.DbKeys.MainConfigId;

}