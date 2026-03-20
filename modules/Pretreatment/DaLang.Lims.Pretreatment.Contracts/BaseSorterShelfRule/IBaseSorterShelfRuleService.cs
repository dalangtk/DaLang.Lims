using DaLang.Lims.Pretreatment.Contracts.BaseSorterShelfRule.Dto;

namespace DaLang.Lims.BaseData.Contracts.BaseSorterShelfRule;

/// <summary>
/// 架子规则服务
/// </summary>
public interface IBaseSorterShelfRuleService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseSorterShelfRuleDto> GetAsync(long id);
    /// <summary>
    /// 列表查询
    /// </summary>
    Task<IEnumerable<BaseSorterShelfRuleDto>> GetListAsync(BaseSorterShelfRuleQueryInput input);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseSorterShelfRuleDto input);
    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseSorterShelfRuleDto input);
    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}
