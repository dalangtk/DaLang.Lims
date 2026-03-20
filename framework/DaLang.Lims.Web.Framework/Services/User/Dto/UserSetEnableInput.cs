namespace DaLang.Lims.Web.Framework.Services.User.Dto;

/// <summary>
/// 设置启用
/// </summary>
public class UserSetEnableInput
{
    /// <summary>
    /// 用户Id
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsValid { get; set; }
}