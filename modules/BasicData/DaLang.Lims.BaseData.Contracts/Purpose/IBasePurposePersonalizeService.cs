using DaLang.Lims.BaseData.Contracts.Purpose.Dto;

namespace DaLang.Lims.BaseData.Contracts.Purpose;

/// <summary>
/// 目的定制服务
/// </summary>
public interface IBasePurposePersonalizeService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BasePurposePersonalizeDto> GetAsync(long id);
    /// <summary>
    /// 根据目的代码查询定制
    /// </summary>
    /// <param name="purcode"></param>
    /// <returns></returns>
    Task<List<BasePurposePersonalizeDto>> GetPurposePersonalizeList(string purcode);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BasePurposePersonalizeDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BasePurposePersonalizeDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}