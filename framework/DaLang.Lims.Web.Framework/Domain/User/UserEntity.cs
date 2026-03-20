using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Org;
using DaLang.Lims.Web.Framework.Domain.Role;
using DaLang.Lims.Web.Framework.Domain.Tenant;
using DaLang.Lims.Web.Framework.Domain.UserOrg;
using DaLang.Lims.Web.Framework.Domain.UserRole;
using DaLang.Lims.Web.Framework.Domain.UserStaff;

namespace DaLang.Lims.Web.Framework.Domain.User;

/// <summary>
/// 用户
/// </summary>
[SugarTable(TableName = "sys_user")]
public partial class UserEntity : EntityTenant
{
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(TenantId), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public TenantEntity Tenant { get; set; }

    /// <summary>
    /// 账号
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 1)]
    public string UserName { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [SugarColumn(Length = 64, CreateTableFieldSort = 2)]
    public string Password { get; set; }

    /// <summary>
    /// 密码加密类型
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 3)]
    public PasswordEncryptType? PasswordEncryptType { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [SugarColumn(Length = 16, CreateTableFieldSort = 4)]
    public string Name { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [SugarColumn(Length = 16, CreateTableFieldSort = 5)]
    public string Mobile { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 6)]
    public string Email { get; set; }

    /// <summary>
    /// 主属部门Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public long OrgId { get; set; }

    /// <summary>
    /// 部门
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(OrgId), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public OrgEntity Org { get; set; }

    /// <summary>
    /// 直属主管Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 8)]
    public long? ManagerUserId { get; set; }

    /// <summary>
    /// 直属主管
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(ManagerUserId), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public UserEntity ManagerUser { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [SugarColumn(Length = 16, IsNullable = true, CreateTableFieldSort = 9)]
    public string NickName { get; set; }

    /// <summary>
    /// 头像
    /// </summary>
    [SugarColumn(Length = 512, IsNullable = true, CreateTableFieldSort = 10)]
    public string Avatar { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 11)]
    public UserStatus? Status { get; set; }

    /// <summary>
    /// 用户类型
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 12)]
    public UserType Type { get; set; } = UserType.DefaultUser;

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 13)]
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 角色列表
    /// </summary>
    [NotGen]
    [Navigate(typeof(UserRoleEntity), nameof(UserRoleEntity.UserId), nameof(UserRoleEntity.RoleId))]
    [SugarColumn(IsIgnore = true)]
    public List<RoleEntity> Roles { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    [NotGen]
    [Navigate(typeof(UserOrgEntity), nameof(UserOrgEntity.UserId), nameof(UserOrgEntity.OrgId))]
    [SugarColumn(IsIgnore = true)]
    public List<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// 员工
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(Id), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public UserStaffEntity Staff { get; set; }
}