using DaLang.Lims.Pathology.Contracts.PathologySamplingSpot.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologySamplingSpot;

/// <summary>
/// 取材部位服务
/// </summary>
public interface IBasePathologySamplingSpotService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<SamplingSpotDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<SamplingSpotDto>> GetPageAsync(PageInput<SamplingSpotQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(SamplingSpotAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(SamplingSpotUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}