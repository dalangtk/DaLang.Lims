using DaLang.Lims.BaseData.Contracts.Purpose.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.BasePurpose;

/// <summary>
/// 检验目的服务
/// </summary>
public interface IBasePurposeService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BasePurposeDto> GetAsync(long id);
    /// <summary>
    /// 获取目的及机构设置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<BasePurposeWithTenantSettingDto> GetPurposeWithTenantSettingAsync(long id);
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BasePurposeGetListDto>> GetPageAsync(PageInput<BasePurposeQueryInput> input);
    /// <summary>
    /// 分页查询2
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<BasePurposeGetListDto>> GetPageWithPersonalizeNameAsync(PageInput<BasePurposeQueryInput> input);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(SavePurposeInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(SavePurposeInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
    /// <summary>
    /// 获取目的明细(已取个性化配置)
    /// </summary>
    /// <param name="purCodes"></param>
    /// <param name="customerCode">客户代码，不传只取机构目的定制</param>
    /// <returns></returns>
    Task<List<BasePurposeDto>> GetPurposeWithPersonalize(List<string> purCodes, string customerCode = "");
    /// <summary>
    /// 获取目的明细，包含上机项目和检验项目
    /// </summary>
    /// <param name="purCodes"></param>
    /// <param name="customerCode"></param>
    /// <returns></returns>
    Task<List<PurposeWithDetailListDto>> GetPurposeDetail(List<string> purCodes, string customerCode = "");
    /// <summary>
    /// 获取目的和套餐
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<PurposeAndComboDto>> GetPurposeAndComboList(QueryPurposeAndComboInput input);
}