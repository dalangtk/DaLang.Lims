using DaLang.Lims.BaseData.Contracts.ExamPlan.Dto;

namespace DaLang.Lims.BaseData.Contracts.ExamPlanDetail;

/// <summary>
/// 检测计划明细服务
/// </summary>
public interface IBaseExamPlanDetailService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<BaseExamPlanDetailDto> GetAsync(long id);

    /// <summary>
    /// 获取所有
    /// </summary>
    Task<List<BaseExamPlanDetailDto>> GetAllAsync(string examPlanCode);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(BaseExamPlanDetailDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(BaseExamPlanDetailDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}