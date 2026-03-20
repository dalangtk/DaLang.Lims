using DaLang.Lims.Pretreatment.Contracts.Handover.Dto;
using DaLang.Lims.Shared.Contracts.ExamTask.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.Handover;

/// <summary>
/// 交接服务
/// </summary>
public interface IHandoverService
{
    /// <summary>
    /// 获取交接任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<GetTaskOutput<List<ExamTaskDto>>> GetTaskAsync(ExamTaskQueryInput input);
    /// <summary>
    /// 获取交接批次
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<GetTaskOutput<List<QueryHandoverBatchNoOutput>>> GetHandoverBatchNo(ExamTaskQueryInput input);
    /// <summary>
    /// 获取批次明细
    /// </summary>
    /// <param name="batchNo"></param>
    /// <returns></returns>
    Task<List<ExamTaskDto>> GetTaskListByBatchNo(long batchNo);
}
