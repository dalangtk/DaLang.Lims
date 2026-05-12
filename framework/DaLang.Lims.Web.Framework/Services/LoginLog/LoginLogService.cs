using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Domain.LoginLog;
using DaLang.Lims.Web.Framework.Services.LoginLog.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using System.Linq;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.LoginLog;

/// <summary>
/// 登录日志服务
/// </summary>
[Order(190)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class LoginLogService : BaseService, ILoginLogService, IDynamicApi
{
    private readonly IHttpContextAccessor _context;
    private readonly ILoginLogRepository _loginLogRep;

    public LoginLogService(
        IHttpContextAccessor context,
        ILoginLogRepository loginLogRepository
    )
    {
        _context = context;
        _loginLogRep = loginLogRepository;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<LoginLogListOutput>> GetPageAsync(PageInput<LogGetPageDto> input)
    {
        var userName = input.Filter?.ProName;

        var list = await _loginLogRep.AsQueryable()
        //.WhereDynamicFilter(input.DynamicFilter)
        .WhereIF(userName.NotNull(), a => a.ProName.Contains(userName))
        .Select<LoginLogListOutput>()
        .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<LoginLogListOutput>()
        {
            List = list.Items.ToList(),
            Total = list.Total
        };

        return data;
    }

    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public async Task<long> AddAsync(LoginLogAddInput input)
    {
        input.IP = IPHelper.GetIP(_context?.HttpContext?.Request);

        string ua = _context.HttpContext.Request.Headers["User-Agent"];
        if (ua.NotNull())
        {
            var client = UAParser.Parser.GetDefault().Parse(ua);
            var device = client.Device.Family;
            device = device.ToLower() == "other" ? "" : device;
            input.Browser = client.UA.Family;
            input.Os = client.OS.Family;
            input.Device = device;
            input.BrowserInfo = ua;
        }
        var entity = Mapper.Map<LoginLogEntity>(input);
        await _loginLogRep.InsertAsync(entity);

        return entity.Id;
    }
}