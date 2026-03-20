using DaLang.Lims.BaseData.Contracts.Instrument.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.Instrument;

/// <summary>
/// 仪器服务
/// </summary>
public interface IBaseInstrumentService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseInstrumentDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseInstrumentGetListDto>> GetPageAsync(PageInput<BaseInstrumentQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseInstrumentDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseInstrumentDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}