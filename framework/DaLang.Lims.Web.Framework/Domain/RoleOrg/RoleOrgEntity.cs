using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Org;
using DaLang.Lims.Web.Framework.Domain.Role;

namespace DaLang.Lims.Web.Framework.Domain;

/// <summary>
/// 角色部门
/// </summary>
[SugarTable(TableName = "sys_role_org")]
public partial class RoleOrgEntity : EntityTenant
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long RoleId { get; set; }

    /// <summary>
    /// 角色
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public RoleEntity Role { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 2)]
    public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public OrgEntity Org { get; set; }
}