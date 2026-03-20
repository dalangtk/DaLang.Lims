using SqlSugar;
using System;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain.UserStaff;

/// <summary>
/// 用户员工
/// </summary>
[SugarTable(TableName = "sys_user_staff")]
public partial class UserStaffEntity : EntityTenant
{
    /// <summary>
    /// 职位
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 1)]
    public string Position { get; set; }

    /// <summary>
    /// 工号
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 2)]
    public string JobNumber { get; set; }

    /// <summary>
    /// 性别
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 3)]
    public Sex? Sex { get; set; }

    /// <summary>
    /// 入职时间
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 4)]
    public DateTime? EntryTime { get; set; }

    /// <summary>
    /// 企业微信名片
    /// </summary>
    [SugarColumn(IsNullable = true, CreateTableFieldSort = 5)]
    public string WorkWeChatCard { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    [SugarColumn(Length = 512, IsNullable = true, CreateTableFieldSort = 1)]
    public string Introduce { get; set; }
}