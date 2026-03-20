using DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood;

/// <summary>
/// 标本分血服务
/// </summary>
public interface IPretreatSortSplitBloodService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<PretreatSortSplitBloodDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<PretreatSortSplitBloodGetListDto>> GetPageAsync(PageInput<PretreatSortSplitBloodQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(PretreatSortSplitBloodDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(PretreatSortSplitBloodDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 标本分血
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<SplitBloodOutput> SplitBlood(SplitBloodInput input);
    /// <summary>
    /// 获取分血明细
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<List<PretreatSortSplitBloodDetailDto>> GetDetailAsync(long id);
}