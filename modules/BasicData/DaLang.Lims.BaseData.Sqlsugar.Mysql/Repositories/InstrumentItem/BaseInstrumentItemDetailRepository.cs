using DaLang.Lims.BaseData.Domain.InstrumentItem;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.BaseData.Sqlsugar.Mysql.Repositories.InstrumentItem;

public class BaseInstrumentItemDetailRepository : AdminRepositoryBase<BaseInstrumentItemDetailEntity>, IBaseInstrumentItemDetailRepository
{
    public BaseInstrumentItemDetailRepository()
    {
    }
}
