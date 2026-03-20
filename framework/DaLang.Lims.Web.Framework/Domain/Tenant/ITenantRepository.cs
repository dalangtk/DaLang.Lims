using DaLang.Lims.Web.Framework.Core.Repositories;

namespace DaLang.Lims.Web.Framework.Domain.Tenant;

public interface ITenantRepository : ISqlSugarRepository<TenantEntity>
{
}