using System.Collections.Generic;

namespace DaLang.Lims.Web.Framework.Services.Auth.Dto;

public class AuthGetUserPermissionsOutput
{
    /// <summary>
    /// 用户个人信息
    /// </summary>
    public AuthUserProfileDto User { get; set; }

    /// <summary>
    /// 用户权限列表
    /// </summary>
    public List<string> Permissions { get; set; }
}