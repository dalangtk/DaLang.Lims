using DaLang.Lims.Api.Contracts;
using DaLang.Lims.Api.Contracts.Dto;
using DaLang.Lims.Statistics.Core.ReportQuery;
using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Services;
using DaLang.Lims.Web.Framework.Services.Auth;
using DaLang.Lims.Web.Framework.Services.Auth.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.Api.Application;

[DynamicApi(Area = ApiConsts.AreaName)]
public class TokenService : BaseService, ITokenService, IDynamicApi
{
    private readonly IAuthService _authService;
    public TokenService(IAuthService authService)
    {
        _authService = authService;
    }
    /// <summary>
    /// 获取token
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("~/auth/token")]
    [AllowAnonymous]
    [Login]
    public async Task<dynamic> GetToken(LoginInput input)
    {
        var param = new AuthLoginInput
        {
            UserName = input.UserName,
            Password = input.Password
        };
        var token = await _authService.LoginAsync(param);
        return token;
    }
}
