using DaLang.Lims.Pathology.Contracts.PathologyDiseaseDetail.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologyDiseaseDetail;

/// <summary>
/// 疾病明细服务
/// </summary>
public interface IBasePathologyDiseaseDetailService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BasePathologyDiseaseDetailDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BasePathologyDiseaseDetailDto>> GetPageAsync(PageInput<BasePathologyDiseaseDetailQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BasePathologyDiseaseDetailAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BasePathologyDiseaseDetailUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}
