using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Services.LoginLog.Dto;

namespace DaLang.Lims.Web.Framework.Services.LoginLog;

/// <summary>
/// 登录日志接口
/// </summary>
public interface ILoginLogService
{
    Task<PageOutput<LoginLogListOutput>> GetPageAsync(PageInput<LogGetPageDto> input);

    Task<long> AddAsync(LoginLogAddInput input);
}