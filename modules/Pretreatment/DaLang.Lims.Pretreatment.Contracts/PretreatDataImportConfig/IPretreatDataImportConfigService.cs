using DaLang.Lims.Pretreatment.Contracts.PretreatDataImportConfig.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.PretreatDataImportConfig;

/// <summary>
/// 导入配置服务
/// </summary>
public interface IPretreatDataImportConfigService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<PretreatDataImportConfigDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<PretreatDataImportConfigGetListDto>> GetPageAsync(PageInput<PretreatDataImportConfigQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(PretreatDataImportConfigDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(PretreatDataImportConfigDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
    /// <summary>
    /// 获取通用导入配置
    /// </summary>
    /// <returns></returns>
    Task<List<PretreatDataImportConfigGetListDto>> GetGeneralImportConfig();
}