using DaLang.Lims.Statistics.Contracts.ReportQuery.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Statistics.Contracts.ReportQuery;

public interface IReportQueryService
{
    /// <summary>
    /// 查询报告
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<ReportQueryOutput>> QueryReport(PageInput<ReportQueryInput> input);
    /// <summary>
    /// 打印报告
    /// </summary>
    /// <param name="examId"></param>
    /// <returns></returns>
    Task<bool> PrintReport(long examId);
}
