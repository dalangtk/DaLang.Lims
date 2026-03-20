using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Document.Dto;

public class DocumentUpdateGroupInput : DocumentAddGroupInput
{
    /// <summary>
    /// 编号
    /// </summary>
    [Required]
    [ValidateRequired("请选择分组")]
    public long Id { get; set; }
}