using DaLang.Lims.Pretreatment.Contracts.SortRule.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.BaseSortRule;

/// <summary>
/// 分拣规则服务
/// </summary>
public interface IBaseSortRuleService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseSortRuleDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseSortRuleGetListDto>> GetPageAsync(PageInput<BaseSortRuleQueryInput> input);

    /// <summary>
    /// 列表查询
    /// </summary>
    Task<IEnumerable<BaseSortRuleGetListDto>> GetListAsync(BaseSortRuleQueryInput input);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseSortRuleDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseSortRuleDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}