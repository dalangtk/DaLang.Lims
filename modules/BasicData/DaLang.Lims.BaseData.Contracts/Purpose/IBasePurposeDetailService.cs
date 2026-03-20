using DaLang.Lims.BaseData.Contracts.Purpose.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.Purpose
{
    /// <summary>
    /// 目的明细服务
    /// </summary>
    public interface IBasePurposeDetailService
    {
        /// <summary>
        /// 查询
        /// </summary>
        Task<BasePurposeDetailDto> GetAsync(long id);
        /// <summary>
        /// 获取目的明细
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<BasePurposeDetailGetListDto>> GetPurposeDetailAsync(BasePurposeDetailQueryInput input);
        /// <summary>
        /// 分页查询
        /// </summary>
        Task<PageOutput<BasePurposeDetailGetListDto>> GetPageAsync(PageInput<BasePurposeDetailQueryInput> input);

        /// <summary>
        /// 新增
        /// </summary>
        Task<long> AddAsync(BasePurposeDetailDto input);

        /// <summary>
        /// 编辑
        /// </summary>
        Task UpdateAsync(BasePurposeDetailDto input);

        /// <summary>
        /// 删除
        /// </summary>
        Task<bool> DeleteAsync(long id);
        /// <summary>
        /// 删除目的明细
        /// </summary>
        /// <param name="purCode">目的代码</param>
        /// <param name="instrumentItemCode">上机项目带啊吗</param>
        /// <returns></returns>
        Task<bool> DeleteByInstrumentItemCodeAsync(string purCode, string instrumentItemCode);
    }
}