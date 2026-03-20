using DaLang.Lims.Web.Framework.Domain.LoginLog;

namespace DaLang.Lims.Web.Framework.Repositories;

public class LoginLogRepository : AdminRepositoryBase<LoginLogEntity>, ILoginLogRepository
{
    public LoginLogRepository()
    {
    }
}