using DaLang.Lims.BaseData.Domain.AuditRule;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.AuditRule;

public class BaseAuditRuleRepository : AdminRepositoryBase<BaseAuditRuleEntity>, IBaseAuditRuleRepository
{
    public BaseAuditRuleRepository()
    {
    }
}
