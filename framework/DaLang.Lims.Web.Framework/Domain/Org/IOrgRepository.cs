using System.Collections.Generic;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Repositories;

namespace DaLang.Lims.Web.Framework.Domain.Org;

public interface IOrgRepository : ISqlSugarRepository<OrgEntity>
{
    /// <summary>
    /// 获得本部门和下级部门Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<long>> GetChildIdListAsync(long id);
}