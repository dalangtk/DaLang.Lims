using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Domain.PermissionApi;

namespace DaLang.Lims.Web.Framework.Domain.Api;

/// <summary>
/// 接口管理
/// </summary>
[SugarTable(TableName = "sys_api")]
public partial class ApiEntity : EntityBase
{
    /// <summary>
    /// 所属模块
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long ParentId { get; set; }

    /// <summary>
    /// 接口命名
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 2)]
    public string Name { get; set; }

    /// <summary>
    /// 接口名称
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 3)]
    public string Label { get; set; }

    /// <summary>
    /// 接口地址
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 4)]
    public string Path { get; set; }

    /// <summary>
    /// 接口提交方法
    /// </summary>
    [SugarColumn(Length = 8, IsNullable = true, CreateTableFieldSort = 5)]
    public string HttpMethods { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 6)]
    public string Description { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public int Sort { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn()]
    public bool IsValid { get; set; } = true;

    [NotGen]
    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    [SugarColumn(IsIgnore = true)]
    public List<ApiEntity> Childs { get; set; }

    [NotGen]
    [Navigate(typeof(PermissionApiEntity), nameof(PermissionApiEntity.ApiId), nameof(PermissionApiEntity.PermissionId))]
    [SugarColumn(IsIgnore = true)]
    public List<PermissionEntity> Permissions { get; set; }
}