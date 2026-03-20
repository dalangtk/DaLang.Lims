using DaLang.Lims.BaseData.Contracts.ExamPlan.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.BaseData.Contracts.ExamPlan;

/// <summary>
/// 检测计划服务
/// </summary>
public interface IBaseExamPlanService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseExamPlanDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<BaseExamPlanDto>> GetPageAsync(PageInput<BaseExamPlanQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseExamPlanDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseExamPlanDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
    /// <summary>
    /// 计算检测计划
    /// </summary>
    /// <param name="planCode"></param>
    /// <param name="receiveTime"></param>
    /// <returns>根据检测计划和接收时间计算检测日期和报告日期,同时返回检测计划的接收时间点estestdate,esreporttime,receivetimepoint</returns>
    Task<(DateTime?, DateTime?, TimeSpan)> CalcTestAndReportDate(string planCode, DateTime receiveTime);
}