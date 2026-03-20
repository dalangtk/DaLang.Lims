using DaLang.Lims.BaseData.Contracts.Item.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Web.BaseData.Contracts.Item;

/// <summary>
/// 基础项目服务
/// </summary>
public interface IBaseItemService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseItemWithPersonDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseItemGetListDto>> GetPageAsync(PageInput<BaseItemQueryInput> input);


    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseItemWithPersonDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseItemWithPersonDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

}