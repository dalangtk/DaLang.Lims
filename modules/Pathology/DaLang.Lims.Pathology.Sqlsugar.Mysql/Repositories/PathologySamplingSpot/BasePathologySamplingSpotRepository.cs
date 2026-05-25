using DaLang.Lims.Pathology.Domain.PathologySamplingSpot;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pathology.Sqlsugar.Mysql.Repositories.PathologySamplingSpot;

public class BasePathologySamplingSpotRepository : AdminRepositoryBase<BasePathologySamplingSpotEntity>, IBasePathologySamplingSpotRepository
{
    public BasePathologySamplingSpotRepository()
    {
    }
}
