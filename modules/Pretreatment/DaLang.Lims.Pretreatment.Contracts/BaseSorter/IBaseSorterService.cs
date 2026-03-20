using DaLang.Lims.Pretreatment.Contracts.BaseSorter.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.BaseSorter;

/// <summary>
/// 分拣仪器服务
/// </summary>
public interface IBaseSorterService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseSorterDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseSorterGetListDto>> GetPageAsync(PageInput<BaseSorterQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(SorterAndDetailAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(SorterAndDetailUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
    /// <summary>
    /// 获取所有分拣仪
    /// </summary>
    /// <returns></returns>
    Task<List<BaseSorterDto>> GetAll();

}