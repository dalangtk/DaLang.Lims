using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.User.Dto;

/// <summary>
/// 修改
/// </summary>
public partial class UserUpdateInput : UserFormInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择用户")]
    public override long Id { get; set; }
}