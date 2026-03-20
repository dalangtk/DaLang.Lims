using DaLang.Lims.Exam.Contracts.ExamCriticalValue.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Exam.Contracts.ExamCriticalValue;

/// <summary>
/// 危急值服务
/// </summary>
public interface IExamCriticalValueService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<ExamCriticalValueDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<ExamCriticalValueDto>> GetPageAsync(PageInput<ExamCriticalValueQueryInput> input);

    /// <summary>
    /// 列表查询
    /// </summary>
    Task<IEnumerable<ExamCriticalValueDto>> GetListAsync(ExamCriticalValueQueryInput input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ExamCriticalValueDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ExamCriticalValueDto input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}