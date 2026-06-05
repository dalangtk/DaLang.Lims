
using DaLang.Lims.Pathology.Domain.PathologySampleType;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pathology.Repositories.PathologySampleType;

public class BasePathologySampleTypeRepository : AdminRepositoryBase<BasePathologySampleTypeEntity>, IBasePathologySampleTypeRepository
{
    public BasePathologySampleTypeRepository()
    {
    }
}
