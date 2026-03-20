using DaLang.Lims.Pretreatment.Contracts.PretreatCustomerPurposeMatch.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.PretreatCustomerPurposeMatch;

/// <summary>
/// 目的对照服务
/// </summary>
public interface IPretreatCustomerPurposeMatchService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<PurposeMatchDto> GetAsync(long id);
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<PurposeMatchGetListDto>> GetPageAsync(PageInput<PurposeMatchQueryInput> input);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(PurposeMatchDto input);
    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(PurposeMatchDto input);
    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
    /// <summary>
    /// 获取客户所有送检的目的
    /// </summary>
    /// <param name="customerCode"></param>
    /// <returns></returns>
    Task<List<PurposeMatchMainDto>> GetCustomerPurMatchMainList(string customerCode);
    /// <summary>
    /// 获取客户指定目的代码的对照明细
    /// </summary>
    /// <param name="customerCode"></param>
    /// <param name="customerPurCode"></param>
    /// <returns></returns>
    Task<List<PurposeMatchDetailDto>> GetCustomerPurMatchDetailList(PurposeMatchMutilQueryInput input);
    /// <summary>
    /// 快速对照
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> QuickPurposeMatch(List<QuickPurmatchInput> input);
}