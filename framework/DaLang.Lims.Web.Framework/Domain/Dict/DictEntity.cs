using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.DictType;

namespace DaLang.Lims.Web.Framework.Domain.Dict;

/// <summary>
/// 数据字典
/// </summary>
[SugarTable("sys_dict")]
public partial class DictEntity : EntityBase
{
    /// <summary>
    /// 字典类型Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long DictTypeId { get; set; }

    /// <summary>
    /// 字典类型
    /// </summary>
    [NotGen]
    [Navigate(NavigateType.OneToOne, nameof(DictTypeId), nameof(DictTypeEntity.Id))]
    [SugarColumn(IsIgnore = true)]
    public DictTypeEntity DictType { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string Name { get; set; }

    /// <summary>
    /// 字典编码
    /// </summary>
    [SugarColumn(Length = 16, CreateTableFieldSort = 3)]
    public string Code { get; set; }

    /// <summary>
    /// 字典值
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 4)]
    public string Value { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 5)]
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 6)]
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public int Sort { get; set; }
}