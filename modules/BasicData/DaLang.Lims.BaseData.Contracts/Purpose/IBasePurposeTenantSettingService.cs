using DaLang.Lims.BaseData.Contracts.Purpose.Dto;

namespace DaLang.Lims.BaseData.Contracts.BasePurpose;

/// <summary>
/// 目的机构设置服务
/// </summary>
public interface IBasePurposeTenantSettingService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BasePurposeTenantSettingDto> GetAsync(long id);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BasePurposeTenantSettingDto input);
    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BasePurposeTenantSettingDto input);
    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}