using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.Web.Framework.Services.Role.Dto;

/// <summary>
/// 添加角色用户列表
/// </summary>
public class RoleAddRoleUserListInput
{
    /// <summary>
    /// 角色
    /// </summary>
    [Required(ErrorMessage = "请选择角色")]
    public long RoleId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    public long[] UserIds { get; set; }
}