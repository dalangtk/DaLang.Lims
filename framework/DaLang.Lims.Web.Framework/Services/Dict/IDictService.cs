using System.Collections.Generic;
using System.Threading.Tasks;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.Dict.Dto;
using DaLang.Lims.Web.Framework.Services.Dict.Dto;
using Microsoft.AspNetCore.Mvc;

namespace DaLang.Lims.Web.Framework.Services.Dict;

/// <summary>
/// 数据字典接口
/// </summary>
public partial interface IDictService
{
    Task<DictGetOutput> GetAsync(long id);

    Task<PageOutput<DictGetPageOutput>> GetPageAsync(PageInput<DictGetPageDto> input);
    Task<Dictionary<string, List<DictGetListDto>>> GetListAsync(string[] codes);
    Task<Dictionary<string, List<DictGetListDto>>> GetListByNamesAsync(string[] names);
    Task<List<DictGetListDto>> GetDictByTypeCodeAsync(string code);
    Task<ActionResult> ExportListAsync();

    Task<long> AddAsync(DictAddInput input);

    Task UpdateAsync(DictUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);
}