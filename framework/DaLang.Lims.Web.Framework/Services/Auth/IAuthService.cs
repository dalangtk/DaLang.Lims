using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Services.Auth.Dto;

namespace DaLang.Lims.Web.Framework.Services.Auth;

/// <summary>
/// 认证授权接口
/// </summary>
public interface IAuthService
{
    string GetToken(AuthLoginOutput user);

    Task<dynamic> LoginAsync(AuthLoginInput input);

    Task<AuthGetUserInfoOutput> GetUserInfoAsync();

    Task<AuthGetPasswordEncryptKeyOutput> GetPasswordEncryptKeyAsync();

    Task<dynamic> Refresh([BindRequired] string token);

}