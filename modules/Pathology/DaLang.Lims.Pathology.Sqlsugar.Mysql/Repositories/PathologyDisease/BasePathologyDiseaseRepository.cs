
using DaLang.Lims.Pathology.Domain.PathologyDisease;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pathology.Repositories.PathologyDisease;

public class BasePathologyDiseaseRepository : AdminRepositoryBase<BasePathologyDiseaseEntity>, IBasePathologyDiseaseRepository
{
    public BasePathologyDiseaseRepository()
    {
    }
}
