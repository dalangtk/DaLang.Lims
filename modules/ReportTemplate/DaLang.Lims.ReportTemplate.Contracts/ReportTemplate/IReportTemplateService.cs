using DaLang.Lims.ReportTemplate.Contracts.ReportTemplate.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.ReportTemplate.Services.ReportTemplate;

/// <summary>
/// 模板服务
/// </summary>
public interface IReportTemplateService
{
    /// <summary>
    /// 查询
    /// </summary>
    Task<ReportTemplateDto> GetAsync(long id);

    /// <summary>
    /// 根据代码查询
    /// </summary>
    /// <param name="templateCode"></param>
    /// <returns></returns>
    Task<ReportTemplateDto> GetByCodeAsync(string templateCode);

    /// <summary>
    /// 分页查询
    /// </summary>
    Task<PageOutput<ReportTemplateDto>> GetPageAsync(PageInput<ReportTemplateQueryInput> input);

    /// <summary>
    /// 新增
    /// </summary>
    Task<long> AddAsync(ReportTemplateDto input);

    /// <summary>
    /// 编辑
    /// </summary>
    Task UpdateAsync(ReportTemplateDto input);

    /// <summary>
    /// 更新模板内容
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> UpdateContentAsync(ReportTemplateUpdateContentInput input);

    /// <summary>
    /// 删除
    /// </summary>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 获取报告数据源
    /// </summary>
    /// <param name="sourceName"></param>
    /// <returns></returns>
    object GetReportData(string sourceName);

    /// <summary>
    /// 获取所有报告模板
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<List<ReportTemplateDto>> GetAllTemplateList(ReportTemplateQueryInput input);

    /// <summary>
    /// 获取检验报告数据源
    /// </summary>
    /// <param name="examInfoId"></param>
    /// <returns></returns>
    Task<ExamReportSourceDto> GetExamReportSource(long examInfoId);

    /// <summary>
    /// 复制模板
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> CopyTemplate(long id);
}