using DaLang.Lims.Pretreatment.Domain.BaseSorterShelf;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pretreatment.Sqlsugar.Mysql.Repositories.BaseSorterShelf;

public class BaseSorterShelfRepository : AdminRepositoryBase<BaseSorterShelfEntity>, IBaseSorterShelfRepository
{
    public BaseSorterShelfRepository()
    {
    }
}
