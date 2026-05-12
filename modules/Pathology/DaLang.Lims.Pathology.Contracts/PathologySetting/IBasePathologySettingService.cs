using DaLang.Lims.Pathology.Contracts.PathologySetting.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.PathologySetting;

/// <summary>
/// 病理配置服务
/// </summary>
public interface IBasePathologySettingService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<PathologySettingDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<PathologySettingDto>> GetPageAsync(PageInput<PathologySettingQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(PathologySettingAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(PathologySettingUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 根据工作流编码查询病理配置
    /// </summary>
    /// <param name="wfCode"></param>
    /// <returns></returns>
    Task<PathologySettingDto> GetSettingByWfCode(string wfCode);
}