using System.Collections.Generic;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Domain.UserOrg;

namespace DaLang.Lims.Web.Framework.Repositories;

public class UserOrgRepository : AdminRepositoryBase<UserOrgEntity>, IUserOrgRepository
{
    public UserOrgRepository()
    {

    }

    /// <summary>
    /// 本部门下是否有员工
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(long id)
    {
        return await IsAnyAsync(a => a.OrgId == id);
    }

    /// <summary>
    /// 部门列表下是否有员工
    /// </summary>
    /// <param name="idList"></param>
    /// <returns></returns>
    public async Task<bool> HasUser(List<long> idList)
    {
        return await IsAnyAsync(a => idList.Contains(a.OrgId));
    }
}