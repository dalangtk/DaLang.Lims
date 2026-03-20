using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Domain.PkgPermission;
using DaLang.Lims.Web.Framework.Domain.Tenant;
using DaLang.Lims.Web.Framework.Domain.TenantPkg;

namespace DaLang.Lims.Web.Framework.Domain.Pkg;

/// <summary>
/// 套餐
/// </summary>
[SugarTable(TableName = "sys_pkg")]
public partial class PkgEntity : EntityTenant
{
    /// <summary>
    /// 父级Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long ParentId { get; set; }

    /// <summary>
    /// 子级列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    [SugarColumn(IsIgnore = true)]
    public List<PkgEntity> Childs { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 3)]
    public string Code { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 4)]
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 5)]
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 6)]
    public int Sort { get; set; }

    /// <summary>
    /// 租户列表
    /// </summary>
    [Navigate(typeof(TenantPkgEntity), nameof(TenantPkgEntity.PkgId), nameof(TenantPkgEntity.TenantId))]
    [SugarColumn(IsIgnore = true)]
    public ICollection<TenantPkgEntity> Tenants { get; set; }

    /// <summary>
    /// 权限列表
    /// </summary>
    [Navigate(typeof(PkgPermissionEntity), nameof(PkgPermissionEntity.PkgId), nameof(PkgPermissionEntity.PermissionId))]
    [SugarColumn(IsIgnore = true)]
    public ICollection<PermissionEntity> Permissions { get; set; }
}