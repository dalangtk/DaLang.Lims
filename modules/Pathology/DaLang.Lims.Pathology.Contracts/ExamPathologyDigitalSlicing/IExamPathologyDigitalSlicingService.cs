using DaLang.Lims.Pathology.Contracts.ExamPathologyDigitalSlicing.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.ExamPathologyDigitalSlicing;

/// <summary>
/// 数字切片服务
/// </summary>
public interface IExamPathologyDigitalSlicingService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<ExamPathologyDigitalSlicingDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<ExamPathologyDigitalSlicingDto>> GetPageAsync(PageInput<ExamPathologyDigitalSlicingQueryInput> input);

    /// <summary>
    /// 列表查询
    /// </summary>
    Task<IEnumerable<ExamPathologyDigitalSlicingDto>> GetListAsync(ExamPathologyDigitalSlicingQueryInput input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ExamPathologyDigitalSlicingAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ExamPathologyDigitalSlicingUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);
}
