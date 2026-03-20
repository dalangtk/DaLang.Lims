using Mapster;
using System.Linq;
using DaLang.Lims.Web.Framework.Services.Tenant.Dto;

namespace DaLang.Lims.Web.Framework.Services.Tenant;

/// <summary>
/// 映射配置
/// </summary>
public class MapConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config
        .NewConfig<TenantListOutput, TenantListOutput>()
        .Map(dest => dest.PkgNames, src => src.Pkgs.Select(a => a.Name));
    }
}