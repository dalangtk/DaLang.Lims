using DaLang.Lims.Web.Framework.Domain.OprationLog;

namespace DaLang.Lims.Web.Framework.Repositories;

public class OprationLogRepository : AdminRepositoryBase<OprationLogEntity>, IOprationLogRepository
{
    public OprationLogRepository()
    {
    }
}