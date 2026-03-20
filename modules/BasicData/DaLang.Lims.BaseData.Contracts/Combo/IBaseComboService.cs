using DaLang.Lims.BaseData.Contracts.Combo.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.Combo;

/// <summary>
/// 套餐管理服务
/// </summary>
public interface IBaseComboService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseComboDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseComboDto>> GetPageAsync(PageInput<BaseComboQueryInput> input);
    /// <summary>
    /// 列表查询
    /// </summary>
    Task<IEnumerable<BaseComboDto>> GetListAsync(BaseComboQueryInput input);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ComboAddOrUpdateInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ComboAddOrUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取套餐明细
    /// </summary>
    /// <param name="comboCode"></param>
    /// <returns></returns>

    Task<List<BaseComboDetailDto>> GetComboDetail(string comboCode);

    /// <summary>
    /// 获取套餐及对应的目的代码
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<BaseComboWithPurcodesDto>> GetComboWithPurcodesListAsync(BaseComboQueryInput input);
}