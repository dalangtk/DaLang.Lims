using DaLang.Lims.Pretreatment.Domain.PretreatSampleInfo;
using DaLang.Lims.Web.Framework.Core.Repositories;

namespace DaLang.Lims.Pretreatment.Domain.PretreatDataImport;

public interface IPretreatDataImportRepository : ISqlSugarRepository<PretreatSampleInfoEntity>
{
}
