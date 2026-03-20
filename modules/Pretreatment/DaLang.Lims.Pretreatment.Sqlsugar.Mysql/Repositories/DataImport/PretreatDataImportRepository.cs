using DaLang.Lims.Pretreatment.Domain.PretreatDataImport;
using DaLang.Lims.Pretreatment.Domain.PretreatSampleInfo;
using DaLang.Lims.Web.Framework.Repositories;

namespace DaLang.Lims.Pretreatment.Sqlsugar.Mysql.Repositories.PretreatDataImport;

public class PretreatDataImportRepository : AdminRepositoryBase<PretreatSampleInfoEntity>, IPretreatDataImportRepository
{
    public PretreatDataImportRepository()
    {
    }
}
