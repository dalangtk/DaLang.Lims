namespace DaLang.Lims.Web.Framework.Services.User.Dto;

/// <summary>
/// 添加
/// </summary>
public class UserAddInput : UserFormInput
{
    /// <summary>
    /// 密码
    /// </summary>
    public virtual string Password { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; } = true;
}