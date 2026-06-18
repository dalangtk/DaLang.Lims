using DaLang.Lims.Pathology.Contracts.ExamPathologySamplingSpot.Dto;

namespace DaLang.Lims.Pathology.Contracts.ExamPathologySamplingSpot;

/// <summary>
/// 病理检测取材部位-标本类型 服务
/// </summary>
public interface IExamPathologySamplingSpotService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<ExamPathologySamplingSpotDto> GetAsync(long id);

    /// <summary>
    /// 列表查询
    /// </summary>
    Task<List<ExamPathologySamplingSpotDto>> GetListAsync(long examInfoId);
    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ExamPathologySamplingSpotAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ExamPathologySamplingSpotUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}
