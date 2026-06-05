using DaLang.Lims.Pathology.Contracts.PathologySubDisease.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologySubDisease;

/// <summary>
/// 子疾病服务
/// </summary>
public interface IBasePathologySubDiseaseService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BasePathologySubDiseaseDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BasePathologySubDiseaseDto>> GetPageAsync(PageInput<BasePathologySubDiseaseQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BasePathologySubDiseaseAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BasePathologySubDiseaseUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}