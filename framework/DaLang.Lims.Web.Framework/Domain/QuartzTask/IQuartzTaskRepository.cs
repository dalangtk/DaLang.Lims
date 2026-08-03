using DaLang.Lims.Web.Framework.Core.Repositories;

namespace DaLang.Lims.Web.Framework.Domain.SysQuartzTask;

public interface IQuartzTaskRepository : ISqlSugarRepository<QuartzTaskEntity>
{
}
