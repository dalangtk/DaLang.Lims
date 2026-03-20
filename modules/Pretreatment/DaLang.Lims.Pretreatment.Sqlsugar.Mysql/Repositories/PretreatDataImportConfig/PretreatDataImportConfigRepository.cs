using DaLang.Lims.Pretreatment.Domain.PretreatDataImportConfig;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pretreatment.Sqlsugar.Mysql.Repositories.PretreatDataImportConfig;

public class PretreatDataImportConfigRepository : AdminRepositoryBase<PretreatDataImportConfigEntity>, IPretreatDataImportConfigRepository
{
    public PretreatDataImportConfigRepository()
    {
    }
}
