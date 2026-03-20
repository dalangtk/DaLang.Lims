using DaLang.Lims.BaseData.Contracts.EntrustHospital.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.EntrustHospital;

/// <summary>
/// 委托医院服务
/// </summary>
public interface IBaseEntrustHospitalService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseEntrustHospitalDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseEntrustHospitalGetListDto>> GetPageAsync(PageInput<BaseEntrustHospitalQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseEntrustHospitalDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseEntrustHospitalDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

}