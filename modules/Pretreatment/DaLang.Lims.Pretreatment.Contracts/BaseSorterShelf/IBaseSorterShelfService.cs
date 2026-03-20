using DaLang.Lims.Pretreatment.Contracts.BaseSorterShelf.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.BaseSorterShelf;

/// <summary>
/// 分拣架子服务
/// </summary>
public interface IBaseSorterShelfService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseSorterShelfDto> GetAsync(long id);

    /// <summary>
    /// 查询分拣仪明细
    /// </summary>
    Task<List<BaseSorterShelfGetListDto>> GetSorterDetailAsync(BaseSorterShelfQueryInput input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseSorterShelfDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseSorterShelfDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}