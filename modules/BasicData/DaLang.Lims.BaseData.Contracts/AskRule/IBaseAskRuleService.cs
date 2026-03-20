using DaLang.Lims.BaseData.Contracts.AskRule.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.AskRule;

/// <summary>
/// 问询规则服务
/// </summary>
public interface IBaseAskRuleService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseAskRuleDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseAskRuleDto>> GetPageAsync(PageInput<BaseAskRuleQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseAskRuleAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseAskRuleUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}