using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain.View;

/// <summary>
/// 视图管理
/// </summary>
[SugarTable(TableName = "sys_view")]
public partial class ViewEntity : EntityBase
{
    /// <summary>
    /// 所属节点
    /// </summary>
    [SugarColumn(IsNullable = false, CreateTableFieldSort = 1)]
    public long ParentId { get; set; }

    /// <summary>
    /// 视图命名
    /// </summary>
    [SugarColumn(IsNullable = true, Length = 64, CreateTableFieldSort = 2)]
    public string Name { get; set; }

    /// <summary>
    /// 视图名称
    /// </summary>
    [SugarColumn(IsNullable = true, Length = 32, CreateTableFieldSort = 3)]
    public string Label { get; set; }

    /// <summary>
    /// 视图路径
    /// </summary>
    [SugarColumn(IsNullable = true, Length = 64, CreateTableFieldSort = 4)]
    public string Path { get; set; }

    /// <summary>
    /// 说明
    /// </summary>
    [SugarColumn(IsNullable = true, Length = 128, CreateTableFieldSort = 5)]
    public string Description { get; set; }

    /// <summary>
    /// 缓存
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 6)]
    public bool Cache { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public int Sort { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 8)]
    public bool IsValid { get; set; } = true;

    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    [SugarColumn(IsIgnore = true)]
    public List<ViewEntity> Childs { get; set; }
}