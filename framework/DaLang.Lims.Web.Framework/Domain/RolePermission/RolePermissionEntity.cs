using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Domain.Role;

namespace DaLang.Lims.Web.Framework.Domain.RolePermission;

/// <summary>
/// 角色权限
/// </summary>
[SugarTable(TableName = "sys_role_permission")]
public class RolePermissionEntity : EntityTenant
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long RoleId { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 2)]
    public long PermissionId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(RoleId))]
    [SugarColumn(IsIgnore = true)]
    public RoleEntity Role { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(PermissionId))]
    [SugarColumn(IsIgnore = true)]
    public PermissionEntity Permission { get; set; }
}