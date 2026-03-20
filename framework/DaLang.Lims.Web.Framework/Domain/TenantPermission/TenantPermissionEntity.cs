using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Domain.Tenant;

namespace DaLang.Lims.Web.Framework.Domain.TenantPermission;

/// <summary>
/// 租户权限
/// </summary>
[SugarTable(TableName = "sys_tenant_permission")]
public class TenantPermissionEntity : EntityTenant
{

    /// <summary>
    /// 权限Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long PermissionId { get; set; }

    /// <summary>
    /// 租户
    /// </summary>
    [NotGen]
    [SugarColumn(IsIgnore = true)]
    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    [NotGen]
    [SugarColumn(IsIgnore = true)]
    public PermissionEntity Permission { get; set; }
}