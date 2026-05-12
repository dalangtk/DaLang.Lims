using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Services.OprationLog.Dto;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.OprationLog;

/// <summary>
/// 操作日志接口
/// </summary>
public interface IOprationLogService
{
    Task<PageOutput<OprationLogListOutput>> GetPageAsync(PageInput<LogGetPageDto> input);

    Task<long> AddAsync(OprationLogAddInput input);
}