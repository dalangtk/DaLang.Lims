using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Role;
using DaLang.Lims.Web.Framework.Domain.User;

namespace DaLang.Lims.Web.Framework.Domain.UserRole;

/// <summary>
/// 用户角色
/// </summary>
[SugarTable(TableName = "sys_user_role")]
public class UserRoleEntity : EntityTenant
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long UserId { get; set; }

    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(UserId), nameof(UserEntity.Id))]
    [SugarColumn(IsIgnore = true)]
    public UserEntity User { get; set; }

    /// <summary>
    /// 角色Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 2)]
    public long RoleId { get; set; }

    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(RoleId), nameof(RoleEntity.Id))]
    [SugarColumn(IsIgnore = true)]
    public RoleEntity Role { get; set; }
}