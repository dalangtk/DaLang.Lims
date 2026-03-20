using DaLang.Lims.BaseData.Domain.SampleType;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.SampleType;

public class BaseSampleTypeRepository : AdminRepositoryBase<BaseSampleTypeEntity>, IBaseSampleTypeRepository
{
    public BaseSampleTypeRepository()
    {
    }
}
