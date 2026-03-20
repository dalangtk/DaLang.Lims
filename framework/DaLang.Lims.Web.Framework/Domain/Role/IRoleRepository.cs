using System.Collections.Generic;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Repositories;

namespace DaLang.Lims.Web.Framework.Domain.Role;

public interface IRoleRepository : ISqlSugarRepository<RoleEntity>
{
    /// <summary>
    /// 获得本角色和下级角色Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long id);

    /// <summary>
    /// 获得当前角色和下级角色Id
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long[] ids);
}