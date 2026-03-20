using System.ComponentModel.DataAnnotations;
using DaLang.Lims.Web.Framework.Core.Validators;
using DaLang.Lims.Web.Framework.Domain.Role;

namespace DaLang.Lims.Web.Framework.Services.Role.Dto;

/// <summary>
/// 设置数据范围
/// </summary>
public class RoleSetDataScopeInput
{
    /// <summary>
    /// 角色Id
    /// </summary>
    [Required]
    [ValidateRequired("请选择角色")]
    public long RoleId { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public DataScope DataScope { get; set; }

    /// <summary>
    /// 指定部门
    /// </summary>
    public long[] OrgIds { get; set; }
}