using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Services.OprationLog.Dto;

namespace DaLang.Lims.Web.Framework.Services.OprationLog;

/// <summary>
/// 操作日志接口
/// </summary>
public interface IOprationLogService
{
    Task<PageOutput<OprationLogListOutput>> GetPageAsync(PageInput<LogGetPageDto> input);

    Task<long> AddAsync(OprationLogAddInput input);
}