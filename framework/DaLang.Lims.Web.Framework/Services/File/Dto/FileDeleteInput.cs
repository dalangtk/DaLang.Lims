
using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Dto;

public class FileDeleteInput
{
    /// <summary>
    /// 文件Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择文件")]
    public long Id { get; set; }
}