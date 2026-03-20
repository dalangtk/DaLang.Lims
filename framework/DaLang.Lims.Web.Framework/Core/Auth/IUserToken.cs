using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DaLang.Lims.Web.Framework.Core.Auth;

public interface IUserToken
{
    string Create(Claim[] claims);

    JwtSecurityToken Decode(string jwtToken);
}