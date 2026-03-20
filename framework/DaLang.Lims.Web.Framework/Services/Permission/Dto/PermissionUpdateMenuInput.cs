using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Permission.Dto;

public class PermissionUpdateMenuInput : PermissionAddMenuInput
{
    /// <summary>
    /// 权限Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择菜单")]
    public long Id { get; set; }
}