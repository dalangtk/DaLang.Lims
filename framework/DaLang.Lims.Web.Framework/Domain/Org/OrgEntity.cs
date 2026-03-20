using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain.Org;

/// <summary>
/// 组织架构
/// </summary>
[SugarTable(TableName = "sys_org")]
public partial class OrgEntity : EntityTenant
{
    /// <summary>
    /// 父级
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 2)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 3)]
    public string Code { get; set; }

    /// <summary>
    /// 值
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 4)]
    public string Value { get; set; }

    /// <summary>
    /// 成员数
    /// </summary>
    [SugarColumn(DefaultValue = "0", CreateTableFieldSort = 5)]
    public int MemberCount { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(DefaultValue = "0", CreateTableFieldSort = 6)]
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public int Sort { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 8)]
    public string Description { get; set; }

    /// <summary>
    /// 员工列表
    /// </summary>
    //[Navigate(ManyToMany = typeof(UserOrgEntity))]

    //public List<UserStaffEntity> Staffs { get; set; }

    /// <summary>
    /// 用户列表
    /// </summary>
    //[NotGen]
    //[Navigate(ManyToMany = typeof(UserOrgEntity))]
    //public List<UserEntity> Users { get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    //[NotGen]
    //[Navigate(ManyToMany = typeof(RoleOrgEntity))]
    //public List<RoleEntity> Roles { get; set; }

    /// <summary>
    /// 子级列表
    /// </summary>
    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    [SugarColumn(IsIgnore = true)]
    public List<OrgEntity> Childs { get; set; }
}