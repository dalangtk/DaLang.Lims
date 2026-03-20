using DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.Sorting;

/// <summary>
/// 分拣服务
/// </summary>
public interface ISortingService
{
    /// <summary>
    /// 开始分拣
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<string> StartSorting(StartSortingInput input);
    /// <summary>
    /// 分拣
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<SortingOutput> Sorting(SortingInput input);
}
