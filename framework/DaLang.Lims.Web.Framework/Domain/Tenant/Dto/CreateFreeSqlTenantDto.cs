using System.Data;

namespace DaLang.Lims.Web.Framework.Domain.Tenant.Dto;

public class CreateFreeSqlTenantDto
{
    /// <summary>
    /// 数据库
    /// </summary>
    public DbType? DbType { get; set; }

    /// <summary>
    /// 连接字符串
    /// </summary>
    public string ConnectionString { get; set; }

    /// <summary>
    /// 空闲时间(分)
    /// </summary>
    public int? IdleTime { get; set; }
}
