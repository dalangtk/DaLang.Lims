using DaLang.Lims.BaseData.Domain.Purpose;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.Purpose;

public class BasePurposeRepository : AdminRepositoryBase<BasePurposeEntity>, IBasePurposeRepository
{
    public BasePurposeRepository()
    {
    }
}
