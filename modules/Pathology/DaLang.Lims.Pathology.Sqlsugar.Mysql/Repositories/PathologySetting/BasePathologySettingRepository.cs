using DaLang.Lims.Pathology.Domain.PathologySetting;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pathology.Sqlsugar.Mysql.Repositories.PathologySetting;

public class BasePathologySettingRepository : AdminRepositoryBase<BasePathologySettingEntity>, IBasePathologySettingRepository
{
    public BasePathologySettingRepository()
    {
    }
}
