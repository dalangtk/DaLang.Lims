using DaLang.Lims.BaseData.Contracts.WorkDate.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Web.BaseData.Contracts.BaseWorkDate;

/// <summary>
/// 工作日服务
/// </summary>
public interface IBaseWorkDateService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseWorkDateDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseWorkDateGetListDto>> GetPageAsync(PageInput<BaseWorkDateQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseWorkDateDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseWorkDateDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

}