using SqlSugar;
using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain.Region;

/// <summary>
/// 地区
/// </summary>
[SugarTable(TableName = "sys_region")]
public partial class RegionEntity : EntityTenant
{
    /// <summary>
    /// 上级Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string Name { get; set; }

    /// <summary>
    /// 级别
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 3)]
    public RegionLevel Level { get; set; }

    /// <summary>
    /// 代码
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 4)]
    public string Code { get; set; }

    /// <summary>
    /// 拼音
    /// </summary>
    [SugarColumn(Length = 16, CreateTableFieldSort = 5)]
    public string Pinyin { get; set; }

    /// <summary>
    /// 拼音首字母
    /// </summary>
    [SugarColumn(Length = 16, CreateTableFieldSort = 6)]
    public string PinyinFirst { get; set; }

    /// <summary>
    /// 提取地址
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 7)]
    public string Url { get; set; }

    /// <summary>
    /// 城乡分类代码
    /// </summary>
    [SugarColumn(Length = 16, CreateTableFieldSort = 8)]
    public string VilageCode { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 9)]
    public int? Sort { get; set; }

    /// <summary>
    /// 热门
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 10)]
    public bool Hot { get; set; } = false;

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 11)]
    public bool IsValid { get; set; } = true;

    [Navigate(NavigateType.OneToMany, nameof(ParentId))]
    [SugarColumn(IsIgnore = true)]
    public List<RegionEntity> Childs { get; set; }
}