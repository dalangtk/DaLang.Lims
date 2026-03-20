using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Domain.Pkg;

namespace DaLang.Lims.Web.Framework.Domain.PkgPermission;

/// <summary>
/// 套餐权限
/// </summary>
[SugarTable(TableName = "sys_pkg_permission")]
public class PkgPermissionEntity : EntityBase
{
    /// <summary>
    /// 套餐Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long PkgId { get; set; }

    /// <summary>
    /// 套餐
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public PkgEntity Pkg { get; set; }

    /// <summary>
    /// 权限Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 2)]
    public long PermissionId { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    [SugarColumn(IsIgnore = true)]
    public PermissionEntity Permission { get; set; }
}