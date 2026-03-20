using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Domain.Document;

namespace DaLang.Lims.Web.Framework.Domain.DocumentImage;

/// <summary>
/// 文档图片
/// </summary>
[SugarTable(TableName = "sys_document_image")]
public class DocumentImageEntity : EntityTenant
{
    /// <summary>
    /// 文档Id
    /// </summary>
    [SugarColumn(CreateTableFieldSort = 1)]
    public long DocumentId { get; set; }
    [SugarColumn(IsIgnore = true)]

    public DocumentEntity Document { get; set; }

    /// <summary>
    /// 请求路径
    /// </summary>
    [SugarColumn(Length = 32, CreateTableFieldSort = 2)]
    public string Url { get; set; }
}