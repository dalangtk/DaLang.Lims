using DaLang.Lims.BaseData.Domain.BaseAskRule;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.AskRule
{
    public class BaseAskRuleRepository : AdminRepositoryBase<BaseAskRuleEntity>, IBaseAskRuleRepository
    {
        public BaseAskRuleRepository()
        {
        }
    }
}
