using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Domain.Pkg;

namespace DaLang.Lims.Web.Framework.Repositories;

public class PkgRepository : AdminRepositoryBase<PkgEntity>, IPkgRepository
{
    public PkgRepository()
    {
    }

    /// <summary>
    /// 获得本角色和下级角色Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await Context.Queryable<PkgEntity>()
        .Where(a => a.Id == id || a.ParentId == id)
        .Select(a => a.Id)
        .ToListAsync();
    }

    /// <summary>
    /// 获得当前角色和下级角色Id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long[] ids)
    {
        return await Context.Queryable<PkgEntity>()
        .Where(a => ids.Contains(a.Id) || ids.Contains(a.ParentId))
        .Select(a => a.Id)
        .ToListAsync();
    }
}