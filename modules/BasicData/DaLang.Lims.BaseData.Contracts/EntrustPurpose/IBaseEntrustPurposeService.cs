using DaLang.Lims.BaseData.Contracts.EntrustPurpose.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Web.BaseData.Contracts.EntrustPurpose
{
    /// <summary>
    /// 委托目的服务
    /// </summary>
    public interface IBaseEntrustPurposeService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<BaseEntrustPurposeDto> GetAsync(long id);
        
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<BaseEntrustPurposeGetListDto>> GetPageAsync(PageInput<BaseEntrustPurposeQueryInput> input);
        /// <summary>
        /// 列表查询
        /// </summary>
        Task<IEnumerable<BaseEntrustPurposeGetListDto>> GetListAsync(BaseEntrustPurposeQueryInput input);
        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(BaseEntrustPurposeDto input);
        
        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(BaseEntrustPurposeDto input);
        
        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteAsync(long id);
    }
}