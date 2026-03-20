using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Domain.User;

namespace DaLang.Lims.Web.Framework.Services.User.Dto;

/// <summary>
/// 添加会员
/// </summary>
public class UserAddMemberInput : UserMemberFormInput
{
    /// <summary>
    /// 密码
    /// </summary>
    [Required(ErrorMessage = "请输入密码")]
    public string Password { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    public UserStatus Status { get; set; }
}