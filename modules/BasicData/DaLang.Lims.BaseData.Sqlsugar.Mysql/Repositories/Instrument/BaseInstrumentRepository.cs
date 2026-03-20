using DaLang.Lims.BaseData.Domain.Instrument;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.Instrument;

public class BaseInstrumentRepository : AdminRepositoryBase<BaseInstrumentEntity>, IBaseInstrumentRepository
{
    public BaseInstrumentRepository()
    {
    }
}
