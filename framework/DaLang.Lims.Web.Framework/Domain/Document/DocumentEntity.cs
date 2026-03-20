using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain.Document;

/// <summary>
/// 文档
/// </summary>
[SugarTable(TableName = "sys_document")]
public partial class DocumentEntity : EntityTenant
{
    /// <summary>
    /// 父级节点
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long ParentId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string Label { get; set; }

    /// <summary>
    /// 类型
    /// </summary>
    [SugarColumn(IsIgnore = true, CreateTableFieldSort = 3)]
    public DocumentType Type { get; set; }

    /// <summary>
    /// 命名
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 4)]
    public string Name { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [SugarColumn(Length = 128, CreateTableFieldSort = 5)]
    public string Content { get; set; }

    /// <summary>
    /// Html
    /// </summary>
    [SugarColumn(Length = 512, CreateTableFieldSort = 6)]
    public string Html { get; set; }

    /// <summary>
    /// 启用
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 7)]
    public bool IsValid { get; set; } = true;

    /// <summary>
    /// 打开组
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 8)]
    public bool? Opened { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 9)]
    public int? Sort { get; set; } = 0;

    /// <summary>
    /// 描述
    /// </summary>
    [SugarColumn(Length = 128, IsNullable = false, CreateTableFieldSort = 10)]
    public string Description { get; set; }
}