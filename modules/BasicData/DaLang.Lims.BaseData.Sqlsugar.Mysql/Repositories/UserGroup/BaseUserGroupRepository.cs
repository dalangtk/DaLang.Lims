using DaLang.Lims.BaseData.Domain.UserGroup;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.UserGroup;

public class BaseUserGroupRepository : AdminRepositoryBase<BaseUserGroupEntity>, IBaseUserGroupRepository
{
    public BaseUserGroupRepository()
    {
    }
}
