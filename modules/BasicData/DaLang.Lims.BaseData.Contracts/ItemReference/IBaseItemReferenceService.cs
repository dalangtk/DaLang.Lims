using DaLang.Lims.BaseData.Contracts.ItemReference.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Web.BaseData.Contracts.ItemReference;

/// <summary>
/// 项目服务
/// </summary>
public interface IBaseItemReferenceService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseItemReferenceDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseItemReferenceDto>> GetPageAsync(PageInput<BaseItemReferenceQueryInput> input);
    /// <summary>
    /// 获取单个项目参考范围
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<BaseItemReferenceDto>> GetItemReferenceAsync(BaseItemReferenceQueryInput input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseItemReferenceDto input);
    /// <summary>
    /// 新增列表
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<bool> AddListAsync(List<BaseItemReferenceDto> list);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseItemReferenceDto input);
    /// <summary>
    /// 保存列表
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    Task<bool> SaveListAsync(List<BaseItemReferenceDto> list);
    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}