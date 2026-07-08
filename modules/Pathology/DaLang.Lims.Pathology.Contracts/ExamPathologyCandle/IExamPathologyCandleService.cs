using DaLang.Lims.Pathology.Contracts.ExamPathologyCandle.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.ExamPathologyCandle;

/// <summary>
/// 蜡块服务
/// </summary>
public interface IExamPathologyCandleService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<ExamPathologyCandleDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<ExamPathologyCandleDto>> GetPageAsync(PageInput<ExamPathologyCandleQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ExamPathologyCandleAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ExamPathologyCandleUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取蜡块
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<List<ExamPathologyCandleDto>> GetCandlesAsync(long examInfoId);
}
