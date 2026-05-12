using DaLang.Lims.Web.Framework.Services.Org.Input;
using DaLang.Lims.Web.Framework.Services.Org.Output;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.Org;

public partial interface IOrgService
{
    Task<OrgGetOutput> GetAsync(long id);

    Task<List<OrgListOutput>> GetListAsync(string key);

    Task<long> AddAsync(OrgAddInput input);

    Task UpdateAsync(OrgUpdateInput input);

    Task DeleteAsync(long id);
}