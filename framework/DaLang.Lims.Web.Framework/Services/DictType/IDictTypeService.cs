using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.DictType.Dto;
using DaLang.Lims.Web.Framework.Services.DictType.Dto;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.DictType;

/// <summary>
/// 数据字典类型接口
/// </summary>
public partial interface IDictTypeService
{
    Task<DictTypeGetOutput> GetAsync(long id);

    Task<PageOutput<DictTypeGetPageOutput>> GetPageAsync(PageInput<DictTypeGetPageDto> input);

    Task<long> AddAsync(DictTypeAddInput input);

    Task UpdateAsync(DictTypeUpdateInput input);

    Task DeleteAsync(long id);

    Task BatchDeleteAsync(long[] ids);
}