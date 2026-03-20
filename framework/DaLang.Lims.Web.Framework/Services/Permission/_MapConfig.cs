using Mapster;
using System.Linq;
using DaLang.Lims.Web.Framework.Domain.Permission;
using DaLang.Lims.Web.Framework.Services.Permission.Dto;

namespace DaLang.Lims.Web.Framework.Services.Admin.Permission;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<PermissionEntity, PermissionGetDotOutput>()
        .Map(dest => dest.ApiIds, src => src.Apis.Select(a => a.Id));
    }
}