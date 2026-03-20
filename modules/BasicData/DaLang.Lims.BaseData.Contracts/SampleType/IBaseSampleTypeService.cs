using DaLang.Lims.BaseData.Contracts.SampleType.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.SampleType;

/// <summary>
/// 标本类型服务
/// </summary>
public interface IBaseSampleTypeService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseSampleTypeDto> GetAsync(long id);
    /// <summary>
    /// 获取所有
    /// </summary>
    /// <returns></returns>
    Task<List<BaseSampleTypeDto>> GetAllAsync();
    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseSampleTypeGetListDto>> GetPageAsync(PageInput<BaseSampleTypeQueryInput> input);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseSampleTypeDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseSampleTypeDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

}