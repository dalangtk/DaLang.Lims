using System.Collections.Generic;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Repositories;

namespace DaLang.Lims.Web.Framework.Domain.UserOrg;

public interface IUserOrgRepository : ISqlSugarRepository<UserOrgEntity>
{
    /// <summary>
    /// 本部门下是否有员工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> HasUser(long id);

    /// <summary>
    /// 部门列表下是否有员工
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    Task<bool> HasUser(List<long> idList);
}