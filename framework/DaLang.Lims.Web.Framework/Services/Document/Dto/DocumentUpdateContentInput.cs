using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Document.Dto;

public class DocumentUpdateContentInput
{
    /// <summary>
    /// 编号
    /// </summary>
    [Required]
    [ValidateRequired("请选择文档")]
    public long Id { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    public string Label { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Html
    /// </summary>
    public string Html { get; set; }
}