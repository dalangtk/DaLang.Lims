using DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.InstrumentItem;

/// <summary>
/// 上机项目明细服务
/// </summary>
public interface IBaseInstrumentItemDetailService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseInstrumentItemDetailDto> GetAsync(long id);
    /// <summary>
    /// 根据上机项目代码获取明细
    /// </summary>
    /// <param name="instrumentItemCodeList"></param>
    /// <returns></returns>
    Task<List<BaseInstrumentItemDetailDto>> GetListByInstrumentItemCodeAsync(List<string> instrumentItemCodeList);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseInstrumentItemDetailGetListDto>> GetPageAsync(PageInput<BaseInstrumentItemDetailQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseInstrumentItemDetailDto input);

    /// <summary>
    /// 新增列表
    /// </summary>
    Task<bool> AddListAsync(List<BaseInstrumentDetailAddInput> input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseInstrumentItemDetailDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

}
