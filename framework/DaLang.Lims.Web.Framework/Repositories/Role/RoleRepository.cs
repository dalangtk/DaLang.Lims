using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Domain.Role;

namespace DaLang.Lims.Web.Framework.Repositories;

public class RoleRepository : AdminRepositoryBase<RoleEntity>, IRoleRepository
{
    public RoleRepository()
    {
    }

    /// <summary>
    /// 获得本角色和下级角色Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long id)
    {
        return await Context.Queryable<RoleEntity>()
        .Where(a => a.Id == id)
        .ToListAsync(a => a.Id);
    }

    /// <summary>
    /// 获得当前角色和下级角色Id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    public async Task<List<long>> GetChildIdListAsync(long[] ids)
    {
        return await Context.Queryable<RoleEntity>()
        .Where(a => ids.Contains(a.Id) || ids.Contains(a.ParentId))
        .ToListAsync(a => a.Id);
    }
}