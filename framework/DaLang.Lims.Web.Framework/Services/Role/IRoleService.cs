using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Role.Dto;
using DaLang.Lims.Web.Framework.Services.Role.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.Role;

/// <summary>
/// 角色接口
/// </summary>
public interface IRoleService
{
    Task<RoleGetOutput> GetAsync(long id);

    Task<List<RoleGetListOutput>> GetListAsync(RoleGetListInput input);

    Task<PageOutput<RoleGetPageOutput>> GetPageAsync(PageInput<RoleGetPageDto> input);

    Task<long> AddAsync(RoleAddInput input);

    Task AddRoleUserAsync(RoleAddRoleUserListInput input);

    Task RemoveRoleUserAsync(RoleAddRoleUserListInput input);

    Task UpdateAsync(RoleUpdateInput input);

    Task DeleteAsync(long id);

    Task SetDataScopeAsync(RoleSetDataScopeInput input);
}