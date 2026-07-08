using DaLang.Lims.Pathology.Contracts.ExamPathologySection.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pathology.Contracts.ExamPathologySection;

/// <summary>
/// 切片服务
/// </summary>
public interface IExamPathologySectionService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<ExamPathologySectionDto> GetAsync(long id);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<ExamPathologySectionDto>> GetPageAsync(PageInput<ExamPathologySectionQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ExamPathologySectionAddInput input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ExamPathologySectionUpdateInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取切片
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<List<ExamPathologySectionDto>> GetSectionsAsync(long examInfoId);
}
