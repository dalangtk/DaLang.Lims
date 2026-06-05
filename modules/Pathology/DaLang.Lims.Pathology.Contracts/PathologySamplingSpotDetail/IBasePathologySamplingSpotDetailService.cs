using DaLang.Lims.Pathology.Contracts.PathologySamplingSpotDetail.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologySamplingSpotDetail;

/// <summary>
/// 取材部位明细服务
/// </summary>
public interface IBasePathologySamplingSpotDetailService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BasePathologySamplingSpotDetailDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BasePathologySamplingSpotDetailDto>> GetPageAsync(PageInput<BasePathologySamplingSpotDetailQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BasePathologySamplingSpotDetailAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BasePathologySamplingSpotDetailUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}
