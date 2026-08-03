using DaLang.Lims.Web.Framework.Core.QuartzTask.Factories;
using DaLang.Lims.Web.Framework.Core.QuartzTask.Options;
using DaLang.Lims.Web.Framework.Domain.SysQuartzTaskLog;
using DaLang.Lims.Web.Framework.Services.QuartzTask;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using Quartz.Impl.Triggers;
using System;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.QuartzTask.Jobs;

public class ClassLibraryJob : IJob
{
    private IQuartzTaskService _quartzService;
    private IServiceProvider _serviceProvider;
    private QuartzTaskOptions _quartzMUIOptions;
    private ILogger<ClassLibraryJob> _logger { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="quartzService"></param>
    /// <param name="logger"></param>
    /// <param name="quartzMUIOptions"></param>
    public ClassLibraryJob(IServiceProvider serviceProvider, IQuartzTaskService quartzService, ILogger<ClassLibraryJob> logger, QuartzTaskOptions quartzMUIOptions)
    {
        _quartzService = quartzService;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _quartzMUIOptions = quartzMUIOptions;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        DateTime dateTime = DateTime.Now;
        string httpMessage = "";
        AbstractTrigger trigger = (context as JobExecutionContextImpl).Trigger as AbstractTrigger;

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
        if (_quartzMUIOptions.ShowConsoleLog)
        {
            _logger.LogInformation($"组别:{trigger.Group},名称:{trigger.Name},的作业开始执行,时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
            Console.WriteLine($"作业[{taskOption.TaskName}]开始:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
        }
        var quartzLog = new QuartzTaskLogEntity() { TenantId = taskOption.TenantId, TaskId = taskOption.Id, BeginDate = DateTime.Now, JobStatus = 0 };
        if (string.IsNullOrEmpty(taskOption.DllName))
        {
            var errmsg = $"组别:{trigger.Group},名称:{trigger.Name},类名不能为空!,时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}";
            _logger.LogError(errmsg);
            return;
        }

        try
        {
            var type = ClassJobsFactory.JobServiceMap[taskOption.DllName];
            var service = (IJobService)_serviceProvider.GetRequiredService(type);
            if (service != null)
            {
                httpMessage = service.ExecuteService(taskOption.TaskParameter);
            }
            else
            {
                httpMessage = "未找到对应类型,请检查是否注入!";
            }
        }
        catch (Exception ex)
        {
            quartzLog.JobStatus = 1;
            httpMessage = ex.Message;
            _logger.LogError($"组别:{trigger.Group},名称:{trigger.Name},执行异常,异常信息:{ex.Message}!,时间:{DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss")}");
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
        if (_quartzMUIOptions.ShowConsoleLog)
        {
            Console.WriteLine(trigger.FullName + " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:sss") + " " + httpMessage);
        }
        return;
    }
}
