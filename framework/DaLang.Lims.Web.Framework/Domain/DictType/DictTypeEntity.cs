using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain.DictType;

/// <summary>
/// 数据字典类型
/// </summary>
[SugarTable(TableName = "sys_dict_type")]
public class DictTypeEntity : EntityBase
{
    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 1)]
    public string Name { get; set; }

    /// <summary>
    /// 编码
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string Code { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = true, CreateTableFieldSort = 3)]
    public string Description { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 4)]
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 5)]
    public int Sort { get; set; }
}