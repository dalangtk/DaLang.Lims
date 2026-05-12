using DaLang.Lims.Pathology.Domain.PathologyTemplate;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pathology.Sqlsugar.Mysql.Repositories.PathologyTemplate;

public class BasePathologyTemplateRepository : AdminRepositoryBase<BasePathologyTemplateEntity>, IBasePathologyTemplateRepository
{
    public BasePathologyTemplateRepository()
    {
    }
}
