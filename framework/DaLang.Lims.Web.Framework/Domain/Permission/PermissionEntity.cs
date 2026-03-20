using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Api;
using DaLang.Lims.Web.Framework.Domain.PermissionApi;
using DaLang.Lims.Web.Framework.Domain.View;

namespace DaLang.Lims.Web.Framework.Domain.Permission;

/// <summary>
/// 权限
/// </summary>
[SugarTable(TableName = "sys_permission")]
public partial class PermissionEntity : EntityBase
{
    /// <summary>
    /// 父级节点
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long ParentId { get; set; }

    /// <summary>
    /// 权限名称
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string Label { get; set; }

    /// <summary>
    /// 权限编码
    /// </summary>
    [SugarColumn(Length = 512, IsNullable = true, CreateTableFieldSort = 3)]
    public string Code { get; set; }

    /// <summary>
    /// 权限类型
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 4)]
    public PermissionType Type { get; set; }

    /// <summary>
    /// 视图Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 5)]
    public long? ViewId { get; set; }

    /// <summary>
    /// 视图
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(ViewId), nameof(Id))]
    [SugarColumn(IsIgnore = true)]
    public ViewEntity View { get; set; }

    /// <summary>
    /// 路由命名
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = true, CreateTableFieldSort = 6)]
    public string Name { get; set; }

    /// <summary>
    /// 路由地址
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = true, CreateTableFieldSort = 7)]
    public string Path { get; set; }

    /// <summary>
    /// 重定向地址
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = true, CreateTableFieldSort = 8)]
    public string Redirect { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    [SugarColumn(Length = 32, IsNullable = true, CreateTableFieldSort = 9)]
    public string Icon { get; set; }

    /// <summary>
    /// 隐藏
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 10)]
    public bool Hidden { get; set; } = false;

    /// <summary>
    /// 展开分组
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 11)]
    public bool Opened { get; set; }

    /// <summary>
    /// 打开新窗口
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 12)]
    public bool NewWindow { get; set; } = false;

    /// <summary>
    /// 链接外显
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 13)]
    public bool External { get; set; } = false;

    /// <summary>
    /// 是否缓存
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 14)]
    public bool IsKeepAlive { get; set; } = true;

    /// <summary>
    /// 是否固定
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 15)]
    public bool IsAffix { get; set; } = false;

    /// <summary>
    /// 链接地址
    /// </summary>
    [SugarColumn(Length = 64, IsNullable = true, CreateTableFieldSort = 16)]
    public string Link { get; set; }

    /// <summary>
    /// 是否内嵌窗口
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 17)]
    public bool IsIframe { get; set; } = false;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 18)]
    public int Sort { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 19)]
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 20)]
    public bool IsValid { get; set; } = true;

    [NotGen]
    [Navigate(typeof(PermissionApiEntity), nameof(PermissionApiEntity.PermissionId), nameof(PermissionApiEntity.ApiId))]
    //[Navigate(NavigateType.OneToMany, nameof(PermissionApiEntity.ApiId))]
    //[SugarColumn(IsIgnore = true)]
    public List<ApiEntity> Apis { get; set; }

    [SqlSugar.Navigate(NavigateType.OneToMany, nameof(ParentId))]
    [SugarColumn(IsIgnore = true)]
    public List<PermissionEntity> Childs { get; set; }
}