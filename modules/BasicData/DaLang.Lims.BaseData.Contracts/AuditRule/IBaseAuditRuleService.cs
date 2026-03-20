using DaLang.Lims.BaseData.Contracts.AuditRule.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.AuditRule;

/// <summary>
/// 审核规则服务
/// </summary>
public interface IBaseAuditRuleService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseAuditRuleDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseAuditRuleDto>> GetPageAsync(PageInput<BaseAuditRuleQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseAuditRuleDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseAuditRuleDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 校验规则
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<TestRuleResultDto> TestRule(TestRuleInput input);

    /// <summary>
    /// 获取规则字段
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<List<string>> GetRuleField(long examInfoId);

    /// <summary>
    /// 获取自定义方法列表
    /// </summary>
    /// <returns></returns>
    List<CustomMethodDto> GetCustomMethodsAsync();
}
