using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Org;
using DaLang.Lims.Web.Framework.Domain.Pkg;
using DaLang.Lims.Web.Framework.Domain.TenantPkg;
using DaLang.Lims.Web.Framework.Domain.User;

namespace DaLang.Lims.Web.Framework.Domain.Tenant;

/// <summary>
/// 租户
/// </summary>
[SugarTable(TableName = "sys_tenant")]
public partial class TenantEntity : EntityBase
{
    /// <summary>
    /// 授权用户
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long UserId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(UserId), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public UserEntity User { get; set; }

    /// <summary>
    /// 授权部门
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 2)]
    public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(OrgId), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// 租户类型
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 3)]
    public int? TenantType { get; set; } = (int)Tenant.TenantType.Tenant;

    /// <summary>
    /// 数据库注册键
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 4)]
    public string DbKey { get; set; }

    /// <summary>
    /// 数据库
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 5)]
    public DbType? DbType { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = true, CreateTableFieldSort = 6)]
    public string ConnectionString { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 说明
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 8)]
    public string Description { get; set; }

    /// <summary>
    /// 套餐列表
    /// </summary>
    [NotGen]
    [Navigate(typeof(TenantPkgEntity), nameof(TenantPkgEntity.TenantId), nameof(TenantPkgEntity.PkgId))]
    [SugarColumn(IsIgnore = true)]
    public List<PkgEntity> Pkgs { get; set; }
}