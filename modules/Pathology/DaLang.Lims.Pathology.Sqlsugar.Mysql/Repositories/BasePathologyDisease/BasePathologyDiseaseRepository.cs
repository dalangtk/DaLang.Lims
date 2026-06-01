
using DaLang.Lims.Pathology.Domain.BasePathologyDisease;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pathology.Repositories.BasePathologyDisease;

public class BasePathologyDiseaseRepository : AdminRepositoryBase<BasePathologyDiseaseEntity>, IBasePathologyDiseaseRepository
{
    public BasePathologyDiseaseRepository()
    {
    }
}
