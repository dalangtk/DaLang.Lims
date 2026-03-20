using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Org;

namespace DaLang.Lims.Web.Framework.Domain.Role;

/// <summary>
/// 角色
/// </summary>
[SugarTable(TableName = "sys_role")]
public partial class RoleEntity : EntityTenant
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
    public List<RoleEntity> Childs { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 3)]
    public string Code { get; set; }

    /// <summary>
    /// 角色类型
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 4)]
    public RoleType Type { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 5)]
    public DataScope DataScope { get; set; } = DataScope.All;

    /// <summary>
    /// 说明
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 6)]
    public string Description { get; set; }

    /// <summary>
    /// 隐藏
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public bool Hidden { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 8)]
    public int Sort { get; set; }

    /// <summary>
    /// 用户列表
    /// </summary>
    //[NotGen]
    ////[Navigate(ManyToMany = typeof(UserRoleEntity))]
    //[SqlSugar.Navigate(typeof(UserRoleEntity),nameof(UserRoleEntity.UserId),nameof(UserRoleEntity.RoleId))]
    //[SugarColumn(IsIgnore = true)]
    //public List<UserEntity> Users { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    [NotGen]
    [Navigate(typeof(RoleOrgEntity), nameof(RoleOrgEntity.RoleId), nameof(RoleOrgEntity.OrgId))]
    [SugarColumn(IsIgnore = true)]
    public List<OrgEntity> Orgs { get; set; }

    /// <summary>
    /// 权限列表
    /// </summary>
    //[NotGen]
    //[Navigate(ManyToMany = typeof(RolePermissionEntity))]
    //[SugarColumn(IsIgnore = true)]
    //public List<PermissionEntity> Permissions { get; set; }
}