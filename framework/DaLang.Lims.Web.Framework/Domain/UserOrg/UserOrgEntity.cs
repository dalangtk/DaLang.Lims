using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Org;
using DaLang.Lims.Web.Framework.Domain.User;

namespace DaLang.Lims.Web.Framework.Domain.UserOrg;

/// <summary>
/// 用户所属部门
/// </summary>
[SugarTable(TableName = "sys_user_org")]
public partial class UserOrgEntity : EntityTenant
{
    /// <summary>
    /// 用户Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long UserId { get; set; }

    /// <summary>
    /// 用户
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(UserId), nameof(UserEntity.Id))]
    [SugarColumn(IsIgnore = true)]
    public UserEntity User { get; set; }

    /// <summary>
    /// 部门Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 2)]
    public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(OrgId), nameof(OrgEntity.Id))]
    [SugarColumn(IsIgnore = true)]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// 是否主管
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 3)]
    public bool IsManager { get; set; } = false;
}