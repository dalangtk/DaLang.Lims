using System.Collections.Generic;
using DaLang.Lims.Web.Framework.Domain.Role;

namespace DaLang.Lims.Web.Framework.Domain.User.Dto;

public class DataPermissionDto
{
    /// <summary>
    /// 部门Id
    /// </summary>
    public long OrgId { get; set; }

    /// <summary>
    /// 部门列表
    /// </summary>
    public List<long> OrgIds { get; set; }

    /// <summary>
    /// 数据范围
    /// </summary>
    public DataScope DataScope { get; set; } = DataScope.Self;
}