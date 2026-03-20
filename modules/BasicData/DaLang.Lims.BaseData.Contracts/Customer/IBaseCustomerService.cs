using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.Customer;

/// <summary>
/// 客户服务
/// </summary>
public interface IBaseCustomerService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<GetCustomerOutput> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseCustomerDto>> GetPageAsync(PageInput<BaseCustomerQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(AddCustomerInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(UpdateCustomerInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
    /// <summary>
    /// 根据客户代码获取客户信息
    /// </summary>
    /// <param name="customerCode"></param>
    /// <returns></returns>
    Task<BaseCustomerDto> GetCustomerInfoByCodeAsync(string customerCode);
}