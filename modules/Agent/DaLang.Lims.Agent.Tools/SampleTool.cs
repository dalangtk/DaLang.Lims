using DaLang.Lims.Agent.Contracts.Dto;
using DaLang.Lims.Agent.Domain;
using DaLang.Lims.Shared.Domain.ExamInfo;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.Framework.Repositories;
using SqlSugar;
using System.ComponentModel;

namespace DaLang.Lims.Agent.Tools;

public class SampleTool : ITools
{
    private readonly ISampleStatisticRepository _statisticRep;
    public SampleTool(ISampleStatisticRepository statisticRep)
    {
        _statisticRep = statisticRep;
    }
    public string ToolName => "sampletool";

    public string Description => "sample quantity statistics";

    [Description("获取时间段内发放的报告数量,按客户和日期分组")]
    public async Task<string> GetReportCountGroupByCusomerAndTestate([Description("查询的起始日期，格式为 yyyy-MM-dd。如果用户说'上个月'，使用上个月1号")] string startTime,
    [Description("查询的截止日期，格式为 yyyy-MM-dd。如果用户说'上个月'，使用上个月最后一天")] string endTime,
    [Description("客户代码,可以为空，为空查所有客户")] string? customerCode)
    {
        var input = new CustomerSampleCountInput
        {
            BeginTime = DateTime.Parse(startTime),
            EndTime = DateTime.Parse(endTime),
            CustomerCode = customerCode
        };

        var ret = await _statisticRep.GetReportCountGroupByCusomerAndTestate(input);
        return JsonHelper.Serialize(ret);
    }
}
