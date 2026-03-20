using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Tenant.Dto;
using DaLang.Lims.Web.Framework.Services.Tenant.Dto;

namespace DaLang.Lims.Web.Framework.Services.Tenant;

/// <summary>
/// 租户接口
/// </summary>
public interface ITenantService
{
    Task<TenantGetOutput> GetAsync(long id);

    Task<PageOutput<TenantListOutput>> GetPageAsync(PageInput<TenantGetPageDto> input);

    Task<long> AddAsync(TenantAddInput input);

    Task UpdateAsync(TenantUpdateInput input);

    Task DeleteAsync(long id);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);
}