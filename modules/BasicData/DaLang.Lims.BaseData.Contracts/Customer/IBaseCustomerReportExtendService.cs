using DaLang.Lims.BaseData.Contracts.Customer.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.Customer
{
    /// <summary>
    /// 客户报告扩展服务
    /// </summary>
    public interface IBaseCustomerReportExtendService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<BaseCustomerReportExtendDto> GetAsync(long id);

        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<BaseCustomerReportExtendDto>> GetPageAsync(PageInput<BaseCustomerReportExtendQueryInput> input);

        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(BaseCustomerReportExtendDto input);

        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(BaseCustomerReportExtendDto input);

        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteAsync(long id);
    }
}