using DaLang.Lims.BaseData.Contracts.InstrumentItem.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.InstrumentItem;

/// <summary>
/// 上机项目服务
/// </summary>
public interface IBaseInstrumentItemService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseInstrumentItemDto> GetAsync(long id);
    /// <summary>
    /// 根据上机项目代码查询
    /// </summary>
    /// <param name="codeList"></param>
    /// <returns></returns>
    Task<List<BaseInstrumentItemDto>> GetByCodeAsync(List<string> codeList);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseInstrumentItemGetListDto>> GetPageAsync(PageInput<BaseInstrumentItemQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseInstrumentItemDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseInstrumentItemDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取上机项目及项目明细
    /// </summary>
    /// <param name="purCodes"></param>
    /// <returns></returns>
    Task<List<InstrumentItemInfoDto>> GetInstrumntItemInfo(List<string> purCodes);
}