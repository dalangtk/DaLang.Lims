using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;

namespace DaLang.Lims.Web.Framework.Services.Org.Input;

/// <summary>
/// 修改
/// </summary>
public class OrgUpdateInput : OrgAddInput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择部门")]
    public long Id { get; set; }
}