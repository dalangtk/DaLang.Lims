using Mapster;
using System.Linq;
using DaLang.Lims.Web.Framework.Services.User.Dto;

namespace DaLang.Lims.Web.Framework.Services.User;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<UserGetPageOutput, UserGetPageOutput>()
        .Map(dest => dest.RoleNames, src => src.Roles.Select(a => a.Name));
    }
}