using DaLang.Lims.BaseData.Domain.Sequence;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.Sequence;

public class BaseSequenceRepository : AdminRepositoryBase<BaseSequenceEntity>, IBaseSequenceRepository
{
    public BaseSequenceRepository()
    {
    }
}
