using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Permission.Dto;

public class PermissionUpdateDotInput : PermissionAddDotInput
{
    /// <summary>
    /// 权限Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择权限点")]
    public long Id { get; set; }
}