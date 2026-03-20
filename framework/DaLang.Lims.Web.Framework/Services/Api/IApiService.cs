using System.Collections.Generic;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Api;
using DaLang.Lims.Web.Framework.Domain.Api.Dto;
using DaLang.Lims.Web.Framework.Services.Api.Dto;

namespace DaLang.Lims.Web.Framework.Services.Api;

/// <summary>
/// api接口
/// </summary>
public interface IApiService
{
    Task<ApiGetOutput> GetAsync(long id);

    Task<List<ApiListOutput>> GetListAsync(string key);

    Task<SqlSugarPagedList<ApiEntity>> GetPageAsync(PageInput<ApiGetPageDto> input);

    Task<long> AddAsync(ApiAddInput input);

    Task UpdateAsync(ApiUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);

    Task SoftDeleteAsync(long id);

    Task BatchSoftDeleteAsync(long[] ids);

    Task SyncAsync(ApiSyncInput input);
}