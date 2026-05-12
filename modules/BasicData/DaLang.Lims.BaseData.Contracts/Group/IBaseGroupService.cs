using DaLang.Lims.BaseData.Contracts.Group.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Web.BaseData.Contracts.Group;

/// <summary>
/// 组别服务
/// </summary>
public interface IBaseGroupService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseGroupDto> GetAsync(long id);
    /// <summary>
    /// 获取所有
    /// </summary>
    /// <returns></returns>
    Task<List<BaseGroupGetListDto>> GetAllAsync(bool includeChildren = false);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseGroupGetListDto>> GetPageAsync(PageInput<BaseGroupQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseGroupDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseGroupDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

}