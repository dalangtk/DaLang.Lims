using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Document.Dto;

public class DocumentUpdateMenuInput : DocumentAddMenuInput
{
    /// <summary>
    /// 编号
    /// </summary>
    [Required]
    [ValidateRequired("请选择菜单")]
    public long Id { get; set; }
}