using System.Collections.Generic;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Domain.Org;

namespace DaLang.Lims.Web.Framework.Repositories;

public class OrgRepository : AdminRepositoryBase<OrgEntity>, IOrgRepository
{
    public OrgRepository()
    {
    }

    /// <summary>
    /// 获得本部门和下级部门Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await Context.Queryable<OrgEntity>()
        .Where(a => a.Id == id || a.ParentId == id)
        .Select(o => o.Id)
        .ToListAsync();
    }
}