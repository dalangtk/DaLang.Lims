using DaLang.Lims.Pretreatment.Domain.SortRule;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pretreatment.Sqlsugar.Mysql.Repositories.SortRule;

public class BaseSortRuleRepository : AdminRepositoryBase<BaseSortRuleEntity>, IBaseSortRuleRepository
{
    public BaseSortRuleRepository()
    {
    }
}
