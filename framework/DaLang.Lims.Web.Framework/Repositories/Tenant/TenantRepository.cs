using DaLang.Lims.Web.Framework.Domain.Tenant;

namespace DaLang.Lims.Web.Framework.Repositories;

public class TenantRepository : AdminRepositoryBase<TenantEntity>, ITenantRepository
{
    public TenantRepository()
    {
    }
}