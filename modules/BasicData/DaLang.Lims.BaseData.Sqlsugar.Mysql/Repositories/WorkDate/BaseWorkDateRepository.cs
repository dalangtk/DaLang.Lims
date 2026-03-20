using DaLang.Lims.Web.BaseData.Domain.BaseWorkDate;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Web.BaseData.Repositories.BaseWorkDate;

public class BaseWorkDateRepository : AdminRepositoryBase<BaseWorkDateEntity>, IBaseWorkDateRepository
{
    public BaseWorkDateRepository()
    {
    }
}
