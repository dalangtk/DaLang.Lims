using DaLang.Lims.BaseData.Contracts.Sequence.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.BaseSequence;

/// <summary>
/// 序列服务
/// </summary>
public interface IBaseSequenceService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseSequenceDto> GetAsync(long id);
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseSequenceGetListDto>> GetPageAsync(PageInput<BaseSequenceQueryInput> input);
    /// <summary>
    /// 列表查询
    /// </summary>
    Task<IEnumerable<BaseSequenceGetListDto>> GetListAsync(BaseSequenceQueryInput input);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseSequenceDto input);
    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseSequenceDto input);
    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}
