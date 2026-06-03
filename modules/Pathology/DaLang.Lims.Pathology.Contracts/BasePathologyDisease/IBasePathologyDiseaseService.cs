using DaLang.Lims.Pathology.Contracts.BasePathologyDisease.Dto;
using DaLang.Lims.Pathology.Contracts.BasePathologyDiseaseDetail.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.BasePathologyDisease;

/// <summary>
/// 疾病服务
/// </summary>
public interface IBasePathologyDiseaseService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BasePathologyDiseaseDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BasePathologyDiseaseDto>> GetPageAsync(PageInput<BasePathologyDiseaseQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BasePathologyDiseaseAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BasePathologyDiseaseUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取疾病明细
    /// </summary>
    /// <param name="diseaseCode"></param>
    /// <returns></returns>
    Task<List<BasePathologyDiseaseDetailDto>> GetDiseaseDetails(string diseaseCode);

    /// <summary>
    /// 新增疾病明细
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> AddDiseaseDetail(List<BasePathologyDiseaseDetailAddInput> input);

    /// <summary>
    /// 删除疾病明细
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteDiseaseDetail(long id);

    /// <summary>
    /// 获取疾病列表
    /// </summary>
    /// <param name="diseaseCodes"></param>
    /// <returns></returns>
    Task<List<BasePathologyDiseaseDto>> GetDiseaseList(List<string> diseaseCodes);
}
