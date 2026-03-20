namespace DaLang.Lims.Web.Framework.Core;

/// <summary>
/// 应用配置
/// </summary>
public class AppSettings
{
    /// <summary>
    /// 使用文件配置
    /// </summary>
    public bool UseFileConfig { get; set; } = false;

    /// <summary>
    /// 配置文件路径
    /// </summary>
    public string FileConfigPath { get; set; } = "Config";
}