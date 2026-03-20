using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Pkg;
using DaLang.Lims.Web.Framework.Domain.Tenant;

namespace DaLang.Lims.Web.Framework.Domain.TenantPkg;

/// <summary>
/// 租户套餐
/// </summary>
[SugarTable(TableName = "sys_tenant_pkg")]
public class TenantPkgEntity : EntityTenant
{

    [SugarColumn(IsIgnore = true)]
    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 套餐Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long PkgId { get; set; }

    [SugarColumn(IsIgnore = true)]
    public PkgEntity Pkg { get; set; }
}