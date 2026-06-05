
using DaLang.Lims.Pathology.Domain.PathologyDiseaseDetail;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pathology.Repositories.PathologyDiseaseDetail;

public class BasePathologyDiseaseDetailRepository : AdminRepositoryBase<BasePathologyDiseaseDetailEntity>, IBasePathologyDiseaseDetailRepository
{
    public BasePathologyDiseaseDetailRepository()
    {
    }
}
