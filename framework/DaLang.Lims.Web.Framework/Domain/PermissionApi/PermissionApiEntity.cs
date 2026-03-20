using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Api;
using DaLang.Lims.Web.Framework.Domain.Permission;

namespace DaLang.Lims.Web.Framework.Domain.PermissionApi;

/// <summary>
/// 权限接口
/// </summary>
[SugarTable(TableName = "sys_permission_api")]
public class PermissionApiEntity : EntityBase//EntityTenant
{
    /// <summary>
    /// 权限Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long PermissionId { get; set; }

    /// <summary>
    /// 权限
    /// </summary>
    [Navigate(NavigateType.ManyToOne, nameof(PermissionId), nameof(PermissionEntity.Id))]
    [SugarColumn(IsIgnore = true)]
    public PermissionEntity Permission { get; set; }

    /// <summary>
    /// 接口Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 2)]
    public long ApiId { get; set; }

    /// <summary>
    /// 接口
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(ApiId), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public ApiEntity Api { get; set; }
}