using DaLang.Lims.BaseData.Contracts.TenantReportExtend.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.TenantReportExtend;

public interface IBaseTenantReportExtendService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseTenantReportExtendDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseTenantReportExtendDto>> GetPageAsync(PageInput<BaseTenantReportExtendQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseTenantReportExtendDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseTenantReportExtendDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}
