using DaLang.Lims.Web.Common.Extensions;
using DaLang.Lims.Web.Framework.Core.QuartzTask.Options;
using DaLang.Lims.Web.Framework.Domain.SysQuartzTaskLog;
using DaLang.Lims.Web.Framework.Services.QuartzTask;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.QuartzTask.Jobs;

public class HttpResultfulJob : IJob
{
    readonly IHttpClientFactory _httpClientFactory;
    private IQuartzTaskService _quartzService;
    private QuartzTaskOptions _quartzOptions;
    private ILogger<HttpResultfulJob> _logger { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="httpClientFactory"></param>
    /// <param name="quartzService"></param>
    /// <param name="logger"></param>
    /// <param name="quartzOptions"></param>
    public HttpResultfulJob(IHttpClientFactory httpClientFactory, IQuartzTaskService quartzService, ILogger<HttpResultfulJob> logger, QuartzTaskOptions quartzOptions)
    {
        _httpClientFactory = httpClientFactory;
        _quartzService = quartzService;
        _logger = logger;
        _quartzOptions = quartzOptions;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        var dateTime = DateTime.Now;
        string httpMessage = "";
        var trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;

        var taskOption = _quartzService.GetJob(trigger.Name, trigger.Group).Result;
        if (taskOption == null)
        {
            taskOption = _quartzService.GetJob(trigger.JobName, trigger.JobGroup).Result;
        }
        if (taskOption == null)
        {
            _logger.LogError($"组别:{trigger.Group},名称:{trigger.Name},的作业未找到,可能已被移除");
            return;
        }
        if (_quartzOptions.ShowConsoleLog)
        {
            if (_quartzOptions.ShowConsoleLog)
            {
                _logger.LogInformation($"组别:{trigger.Group},名称:{trigger.Name},的作业开始执行,时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
                Console.WriteLine($"作业[{taskOption.TaskName}]开始:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
            }
        }
        var quartzLog = new QuartzTaskLogEntity() { TenantId = taskOption.TenantId, TaskId = taskOption.Id, BeginDate = DateTime.Now, JobStatus = 0 };
        if (string.IsNullOrEmpty(taskOption.ApiUrl) || taskOption.ApiUrl == "/")
        {
            _logger.LogError($"组别:{trigger.Group},名称:{trigger.Name},参数非法或者异常!,时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
            return;
        }

        try
        {
            var header = new Dictionary<string, string>();
            var taskHeader = taskOption.ApiRequestHeader;
            if (!string.IsNullOrEmpty(taskHeader))
            {
                foreach (var item in taskHeader.Split(Environment.NewLine))
                {
                    header.Add(item.Split(':')[0].Trim(), item.Split(':')[1].Trim());
                }
            }

            httpMessage = await _httpClientFactory.HttpSendAsync(
                taskOption.ApiRequestType?.ToLower() == "get" ? HttpMethod.Get : HttpMethod.Post,
                taskOption.ApiUrl,
                taskOption.TaskParameter,
                header, taskOption.ApiTimeOut ?? _quartzOptions.DefaultApiTimeOut);
        }
        catch (Exception ex)
        {
            quartzLog.JobStatus = 1;
            httpMessage = ex.Message;
            _logger.LogError($"组别:{trigger.Group},名称:{trigger.Name},执行异常,异常信息:{ex.Message}!,时间:{DateTime.Now:yyyy-MM-dd HH:mm:sss}");
        }
        try
        {
            quartzLog.EndDate = DateTime.Now;
            quartzLog.DurationMs = (int)(quartzLog.EndDate - quartzLog.BeginDate)?.TotalMilliseconds;
            quartzLog.Msg = httpMessage;
            await _quartzService.AddLogAsync(quartzLog);
        }
        catch (Exception)
        {
        }
        if (_quartzOptions.ShowConsoleLog)
        {
            Console.WriteLine(trigger.FullName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + " " + httpMessage);
        }
        return;
    }
}
