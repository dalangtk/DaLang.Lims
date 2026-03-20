namespace DaLang.Lims.Web.Framework.Core.Entities;

/// <summary>
/// 删除接口
/// </summary>
public interface IDeletedFilter
{
    /// <summary>
    /// 软删除
    /// </summary>
    bool IsDeleted { get; set; }
}

/// <summary>
/// 租户Id接口过滤器
/// </summary>
public interface ITenantIdFilter
{
    /// <summary>
    /// 租户Id
    /// </summary>
    long? TenantId { get; set; }
}

/// <summary>
/// 机构Id接口过滤器
/// </summary>
public interface IOrgIdFilter
{
    /// <summary>
    /// 创建者部门Id
    /// </summary>
    long? CreateOrgId { get; set; }
}