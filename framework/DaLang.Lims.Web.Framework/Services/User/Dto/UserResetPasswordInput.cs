using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Services.User.Dto;

/// <summary>
/// 重置密码
/// </summary>
public class UserResetPasswordInput : EntityBaseId
{
    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; }
}