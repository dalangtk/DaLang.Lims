using DaLang.Lims.Pretreatment.Domain.BaseSorter;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pretreatment.Sqlsugar.Mysql.Repositories.BaseSorter;

public class BaseSorterRepository : AdminRepositoryBase<BaseSorterEntity>, IBaseSorterRepository
{
    public BaseSorterRepository()
    {
    }
}
