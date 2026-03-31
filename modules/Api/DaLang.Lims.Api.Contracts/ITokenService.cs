using DaLang.Lims.Api.Contracts.Dto;

namespace DaLang.Lims.Api.Contracts;

public interface ITokenService
{
    Task<dynamic> GetToken(LoginInput input);
}
