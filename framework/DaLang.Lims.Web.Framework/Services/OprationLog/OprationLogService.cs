using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain;
using DaLang.Lims.Web.Framework.Domain.OprationLog;
using DaLang.Lims.Web.Framework.Services.OprationLog.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.OprationLog;

/// <summary>
/// 操作日志服务
/// </summary>
[Order(200)]
[DynamicApi(Area = AdminConsts.AreaName)]
public class OprationLogService : BaseService, IOprationLogService, IDynamicApi
{
    private readonly IHttpContextAccessor _context;
    private readonly IOprationLogRepository _oprationLogRep;

    public OprationLogService(
        IHttpContextAccessor context,
        IOprationLogRepository oprationLogRep
    )
    {
        _context = context;
        _oprationLogRep = oprationLogRep;
    }

    /// <summary>
    /// 查询分页
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<OprationLogListOutput>> GetPageAsync(PageInput<LogGetPageDto> input)
    {
        var userName = input.Filter?.ProName;
        var dynamicCondition = ChangeConditon(input.DynamicFilter);

        var list = await _oprationLogRep.GetQueryable(dynamicCondition)
        .WhereIF(userName.NotNull(), a => a.ProName.Contains(userName))
        .Select<OprationLogListOutput>()
        .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<OprationLogListOutput>()
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
    public async Task<long> AddAsync(OprationLogAddInput input)
    {
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

        input.Name = User.Name;
        input.IP = IPHelper.GetIP(_context?.HttpContext?.Request);

        var entity = Mapper.Map<OprationLogEntity>(input);
        await _oprationLogRep.InsertAsync(entity);

        return entity.Id;
    }
}