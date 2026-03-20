using DaLang.Lims.BaseData.Contracts.AskRule.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Web.BaseData.Services.BaseAskRuleDetail;

/// <summary>
/// 问询规则明细服务
/// </summary>
public interface IBaseAskRuleDetailService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseAskRuleDetailDto> GetAsync(long id);

    /// <summary>
    /// 根据代码查询明细
    /// </summary>
    Task<List<BaseAskRuleDetailDto>> GetAllAsync(string aksRuleCode);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseAskRuleDetailDto>> GetPageAsync(PageInput<BaseAskRuleDetailQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseAskRuleDetailAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseAskRuleDetailUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}