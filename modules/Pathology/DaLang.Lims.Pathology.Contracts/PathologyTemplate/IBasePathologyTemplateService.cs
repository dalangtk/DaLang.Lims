using DaLang.Lims.Pathology.Contracts.PathologyTemplate.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologyTemplate;

/// <summary>
/// 诊断模板服务
/// </summary>
public interface IBasePathologyTemplateService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<PathologyTemplateDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<PathologyTemplateDto>> GetPageAsync(PageInput<PathologyTemplateQueryInput> input);

    /// <summary>
    /// 根据工作流获取所有模板
    /// </summary>
    /// <param name="wfCode"></param>
    /// <returns></returns>
    Task<List<PathologyTemplateDto>> GetListByWfCode(string wfCode);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(PathologyTemplateDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(PathologyTemplateDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 根据模板代码获取模板列表
    /// </summary>
    /// <param name="templateCodes"></param>
    /// <returns></returns>
    Task<List<LabelValueDto>> GetPathologyTemplateList(List<string> templateCodes);
}
