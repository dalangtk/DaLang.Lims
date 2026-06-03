using DaLang.Lims.Pathology.Contracts.BasePathologySampleType.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.BasePathologySampleType;

/// <summary>
/// 病理标本服务
/// </summary>
public interface IBasePathologySampleTypeService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BasePathologySampleTypeDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BasePathologySampleTypeDto>> GetPageAsync(PageInput<BasePathologySampleTypeQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BasePathologySampleTypeAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BasePathologySampleTypeUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取标本类型列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<BasePathologySampleTypeDto>> GetSampleTypeList(BasePathologySampleTypeQueryInput input);
}
