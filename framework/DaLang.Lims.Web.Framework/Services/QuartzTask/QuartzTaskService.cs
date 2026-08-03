using DaLang.Lims.Web.DynamicApi;
using DaLang.Lims.Web.DynamicApi.Attributes;
using DaLang.Lims.Web.Framework.Contracts.SysQuartzTaskLog.Dto;
using DaLang.Lims.Web.Framework.Core.Consts;
using DaLang.Lims.Web.Framework.Core.Db.SqlSugar;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Core.Entities;
using DaLang.Lims.Web.Framework.Core.QuartzTask.Enums;
using DaLang.Lims.Web.Framework.Core.QuartzTask.Jobs;
using DaLang.Lims.Web.Framework.Domain.SysQuartzTask;
using DaLang.Lims.Web.Framework.Domain.SysQuartzTaskLog;
using DaLang.Lims.Web.Framework.Services.QuartzTask.Dto;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using Quartz.Spi;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.QuartzTask;

/// <summary>
/// 定时任务服务
/// </summary>
[DynamicApi(Area = AdminConsts.AreaName)]
public class QuartzTaskService : BaseService, IQuartzTaskService, IDynamicApi

{
    private IQuartzTaskRepository _quartzTaskRep;
    private IQuartzTaskLogRepository _quartzTaskLogRep;
    private ISchedulerFactory _schedulerFactory;
    private IJobFactory _jobFactory;
    private ILogger<QuartzTaskService> _logger;

    public QuartzTaskService(IQuartzTaskRepository quartzTaskRep,
        IQuartzTaskLogRepository quartzTaskLogRep,
        ISchedulerFactory schedulerFactory,
        IJobFactory jobFactory,
        ILogger<QuartzTaskService> logger)
    {
        _quartzTaskRep = quartzTaskRep;
        _quartzTaskLogRep = quartzTaskLogRep;
        _schedulerFactory = schedulerFactory;
        _jobFactory = jobFactory;
        _logger = logger;
    }
    public QuartzTaskService() { }

    /// <summary>
    /// 获取作业
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<QuartzTaskDto> GetAsync(long id)
    {
        var job = await _quartzTaskRep.GetByIdAsync(id);
        return job.Adapt<QuartzTaskDto>();
    }
    /// <summary>
    /// 获取作业列表
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<List<QuartzTaskDto>> GetListAsync(QuartzTaskQueryInput input)
    {
        List<QuartzTaskDto> list = new();
        try
        {
            IScheduler _scheduler = await _schedulerFactory.GetScheduler();
            var groups = await _scheduler.GetJobGroupNames();

            list = await _quartzTaskRep.AsQueryable()
                .WhereIF(!string.IsNullOrWhiteSpace(input.TaskName), a => a.TaskName.Contains(input.TaskName))
                .WhereIF(!string.IsNullOrWhiteSpace(input.GroupName), a => a.GroupName.Contains(input.GroupName))
                .WhereIF(!string.IsNullOrWhiteSpace(input.TaskOrGroupName), a => a.TaskName.Contains(input.TaskOrGroupName) || a.GroupName.Contains(input.TaskOrGroupName))
                .ClearFilter<ITenantIdFilter>()
                .Select<QuartzTaskDto>()
                .ToListAsync();

            foreach (var groupName in groups)
            {
                var jobs = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName));
                foreach (var jobKey in jobs)
                {
                    var taskOption = list.Where(x => x.GroupName == jobKey.Group && x.TaskName == jobKey.Name)
                        .FirstOrDefault();
                    if (taskOption == null)
                        continue;

                    var triggers = await _scheduler.GetTriggersOfJob(jobKey);
                    foreach (ITrigger trigger in triggers)
                    {
                        DateTimeOffset? dateTimeOffset = trigger.GetPreviousFireTimeUtc();
                        if (dateTimeOffset != null)
                        {
                            taskOption.LastRunTime = Convert.ToDateTime(dateTimeOffset.ToString());
                        }
                        else
                        {
                            var runlog = await _quartzTaskLogRep.AsQueryable()
                                .Where(a => a.TaskId == taskOption.Id)
                                .OrderByDescending(a => a.ProTime)
                                .FirstAsync();
                            if (runlog != null)
                                taskOption.LastRunTime = runlog.BeginDate;
                        }
                        var nextFireTime = Convert.ToDateTime(trigger.GetNextFireTimeUtc().ToString());
                        taskOption.NextRunTime = nextFireTime;
                        var state = await _scheduler.GetTriggerState(trigger.Key);
                        taskOption.StateDisplay = state switch
                        {
                            TriggerState.Normal => "正常",
                            TriggerState.Paused => "暂停",
                            TriggerState.Complete => "完成",
                            TriggerState.Error => "异常",
                            TriggerState.Blocked => "阻塞",
                            TriggerState.None => "不存在",
                            _ => "未知",
                        };
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (_logger != null)
            {
                _logger.LogWarning("获取作业异常：" + ex.Message + ex.StackTrace);
            }
        }
        return list;
    }

    [NonAction]
    public async Task<QuartzTaskEntity> GetJob(string taskName, string groupName)
    {
        var job = await _quartzTaskRep.AsQueryable()
                .Where(a => a.TaskName == taskName && a.GroupName == groupName)
                .FirstAsync();
        return job;
    }

    [NonAction]
    public async Task InitJobs()
    {
        var jobs = await _quartzTaskRep.GetListAsync();
        IScheduler scheduler = await _schedulerFactory.GetScheduler();
        foreach (var item in jobs)
        {
            try
            {
                IJobDetail job = CreateJob(item);//null;
                if (item.TaskType == 2)
                {
                    job = JobBuilder.Create<ClassLibraryJob>()
                    .WithIdentity(item.TaskName, item.GroupName)
                    .Build();

                }
                else
                {
                    job = JobBuilder.Create<HttpResultfulJob>()
                    .WithIdentity(item.TaskName, item.GroupName)
                    .Build();
                }

                var builder = TriggerBuilder.Create()
                  .WithIdentity(item.TaskName, item.GroupName)
                  .WithDescription(item.Describe);

                if (item.TriggerType == TriggerTypeEnum.Simple)
                {
                    builder = builder.WithSimpleSchedule((b) =>
                    {
                        b.WithIntervalInSeconds(int.Parse(item.Interval));
                        b.WithRepeatCount(-1);
                    });
                }
                else
                {
                    builder = builder.WithCronSchedule(item.Interval);
                }

                ITrigger trigger = builder.Build();
                if (_jobFactory != null)
                {
                    scheduler.JobFactory = _jobFactory;
                }


                if (item.Status == (int)JobStateEnum.Start)
                {
                    await scheduler.ScheduleJob(job, trigger);
                    await AddLogAsync(new QuartzTaskLogEntity()
                    {
                        TaskId = item.Id,
                        BeginDate = DateTime.Now,
                        Msg = $"任务初始化启动成功:{item.Status}",
                        TenantId = item.TenantId,
                        ProId = item.ProId,
                        ProName = item.ProName,
                        ProTime = DateTime.Now,
                    });
                }
                else
                {
                    await scheduler.ScheduleJob(job, trigger);
                    await PauseAsync(item);
                    _logger.LogInformation($"任务初始化,未启动,状态为:{item.Status}");
                }
            }
            catch (Exception ex)
            {
                await AddLogAsync(new QuartzTaskLogEntity()
                {
                    TaskId = item.Id,
                    BeginDate = DateTime.Now,
                    Msg = $"任务初始化未启动,出现异常,异常信息{ex.Message}",
                    TenantId = item.TenantId,
                    ProId = item.ProId,
                    ProName = item.ProName,
                    ProTime = DateTime.Now,
                });
                continue;
            }
        }

        await scheduler.Start();
    }
    /// <summary>
    /// 添加任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> AddAsync(QuartzTaskAddInput input)
    {
        if (input.TriggerType == TriggerTypeEnum.Cron)
        {
            bool validExpression = false;
            try
            {
                validExpression = IsValidExpression(input.Interval);
            }
            catch (Exception)
            {
                validExpression = false;
            }
            if (!validExpression)
                throw ResultOutput.Exception($"请确认表达式{input.Interval}是否正确!");
        }

        var entity = Mapper.Map<QuartzTaskEntity>(input);

        var model = await _quartzTaskRep.AsQueryable()
            .Where(a => a.TaskName == input.TaskName && a.GroupName == input.GroupName)
            .ClearFilter<ITenantIdFilter>()
            .FirstAsync();
        if (model != null)
            throw ResultOutput.Exception($"{input.GroupName}-{input.TaskName}任务已存在,添加失败!");

        if (entity.Sort == 0)
        {
            var sort = await _quartzTaskRep.AsQueryable().MaxAsync(a => a.Sort);
            entity.Sort = sort + 1;
        }
        entity = await _quartzTaskRep.InsertReturnEntityAsync(entity);
        IJobDetail job = CreateJob(entity);
        ITrigger trigger = CreateTrigger(entity);

        IScheduler scheduler = await _schedulerFactory.GetScheduler();
        if (_jobFactory != null)
            scheduler.JobFactory = _jobFactory;

        //开启才加入Schedule中,如果加入在暂停而定时任务执行过快,会导致卡死
        if (entity.Status == (int)JobStateEnum.Start)
        {
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
        else
        {
            await PauseAsync(entity);
            await AddLogAsync(new QuartzTaskLogEntity() { TaskId = entity.Id, Msg = $"任务新建,未启动,状态为:{entity.Status}" });
        }
        return true;
    }

    /// <summary>
    /// 更新任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> UpdateAsync(QuartzTaskUpdateInput input)
    {
        var entity = await _quartzTaskRep.GetAsync(input.Id);
        if (!(entity?.Id > 0))
            throw ResultOutput.Exception("定时任务不存在！");

        var jobExists = await IsQuartzJob(entity.TaskName, entity.GroupName);
        if (jobExists.Item1)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(entity.GroupName)).Result.ToList();
            JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result.Any(x =>
            {
                if ((x is CronTriggerImpl cti) && cti.Name == entity.TaskName)
                {
                    return true;
                }
                else if ((x is SimpleTriggerImpl sti) && sti.Name == entity.TaskName)
                {
                    return true;
                }
                return false;
            })).FirstOrDefault();
            var triggers = await scheduler.GetTriggersOfJob(jobKey);
            ITrigger triggerold = triggers?.Where(x =>
            {
                if ((x is CronTriggerImpl cti) && cti.Name == entity.TaskName)
                {
                    return true;
                }
                else if ((x is SimpleTriggerImpl sti) && sti.Name == entity.TaskName)
                {
                    return true;
                }
                return false;
            }).FirstOrDefault();
            await scheduler.PauseTrigger(triggerold.Key);
            await scheduler.UnscheduleJob(triggerold.Key);// 移除触发器
            await scheduler.DeleteJob(triggerold.JobKey);

            IJobDetail job = null;
            Mapper.Map(input, entity);
            if (entity.TaskType == 2)
            {
                job = JobBuilder.Create<ClassLibraryJob>()
                .WithIdentity(entity.TaskName, entity.GroupName)
                .WithDescription(entity.Id.ToString())
                .Build();
            }
            else
            {
                job = JobBuilder.Create<HttpResultfulJob>()
                .WithIdentity(entity.TaskName, entity.GroupName)
                .WithDescription(entity.Id.ToString())
                .Build();
            }
            var builder = TriggerBuilder.Create()
                  .WithIdentity(entity.TaskName, entity.GroupName)
                  .WithDescription(entity.Id.ToString())
                  .WithDescription(entity.Describe);

            if (entity.TriggerType == TriggerTypeEnum.Simple)
            {
                builder = builder.WithSimpleSchedule((b) =>
                {
                    b.WithIntervalInSeconds(int.Parse(entity.Interval));
                    b.WithRepeatCount(-1);
                });
            }
            else
            {
                builder = builder.WithCronSchedule(entity.Interval);
            }

            ITrigger triggernew = builder.Build();
            if (_jobFactory != null)
            {
                scheduler.JobFactory = _jobFactory;
            }
            await scheduler.ScheduleJob(job, triggernew);
            if (entity.Status == (int)JobStateEnum.Start)
            {
                await scheduler.Start();
            }
            else
            {
                await scheduler.PauseTrigger(triggernew.Key);
                await AddLogAsync(new QuartzTaskLogEntity() { TaskId = entity.Id, Msg = $"任务新建,未启动,状态为:{entity.Status}" });
            }
        }
        var ret = await _quartzTaskRep.UpdateAsync(entity);
        return ret;
    }

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    [HttpGet]
    public async Task<bool> DeleteAsync(long id)
    {
        var entity = await _quartzTaskRep.GetFirstAsync(a => a.Id == id);
        if (entity == null)
            throw ResultOutput.Exception("任务不存在！");

        var jobExists = await IsQuartzJob(entity.TaskName, entity.GroupName);
        if (jobExists.Item1)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(entity.GroupName)).Result.ToList();
            JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result.Any(x =>
            {
                if ((x is CronTriggerImpl cti) && cti.Name == entity.TaskName)
                {
                    return true;
                }
                else if ((x is SimpleTriggerImpl sti) && sti.Name == entity.TaskName)
                {
                    return true;
                }
                return false;
            })).FirstOrDefault();
            var triggers = await scheduler.GetTriggersOfJob(jobKey);
            ITrigger trigger = triggers?.Where(x =>
            {
                if ((x is CronTriggerImpl cti) && cti.Name == entity.TaskName)
                {
                    return true;
                }
                else if ((x is SimpleTriggerImpl sti) && sti.Name == entity.TaskName)
                {
                    return true;
                }
                return false;
            }).FirstOrDefault();
            await scheduler.PauseTrigger(trigger.Key);
            await scheduler.UnscheduleJob(trigger.Key);// 移除触发器
            await scheduler.DeleteJob(trigger.JobKey);
        }

        return await _quartzTaskRep
            .SetColumnUpdateable(a => a.IsDeleted == true)
            .Where(a => a.Id == id)
            .ExecuteCommandAsync() > 0;
    }
    [HttpGet]
    public async Task<QuartzTaskDashboardDto> GetDashboardInfo()
    {
        var result = new QuartzTaskDashboardDto();
        //查询当天任务执行次数
        result.JobCounts = await _quartzTaskLogRep.AsQueryable().Where(a => a.BeginDate >= DateTime.Today).CountAsync();
        //查询运行中的任务数量/任务总数
        result.RunJobs = string.Join("/", _quartzTaskRep.AsQueryable().Where(a => a.Status == 6).Count(), _quartzTaskRep.AsQueryable().Count());
        //查询当天任务执行平均耗时
        var logs = await _quartzTaskLogRep.AsQueryable().Where(a => a.BeginDate >= DateTime.Today).ToListAsync();
        result.AverageTime = logs?.Select(a => a.DurationMs)?.DefaultIfEmpty(0)?.Average() ?? 0;   // 如果 logs 为空，默认值 0
                                                                                                   //查询当天任务错误率
        var daylogs = await _quartzTaskLogRep.AsQueryable().Where(a => a.BeginDate >= DateTime.Today).Select(a => a.JobStatus).ToListAsync();
        int logcount = daylogs.Count;
        int errorcount = daylogs.Where(a => a == 1).Count();
        result.ErrorCounts = logcount == 0 ? 0 : ((double)errorcount / logcount) * 100;
        return result;
    }

    [HttpGet]
    public async Task<dynamic> GetDurationDistribution()
    {
        var today = DateTime.Today;
        var todayLogs = await _quartzTaskLogRep.AsQueryable()
            .Where(x => x.BeginDate >= today && x.DurationMs > 0).ToListAsync();

        var result = new
        {
            Fast = todayLogs.Count(x => x.DurationMs < 100),
            Normal = todayLogs.Count(x => x.DurationMs >= 100 && x.DurationMs < 500),
            Slow = todayLogs.Count(x => x.DurationMs >= 500 && x.DurationMs < 1000),
            VerySlow = todayLogs.Count(x => x.DurationMs >= 1000)
        };
        return result;
    }

    [HttpGet]
    public async Task<List<dynamic>> GetErrorTop10()
    {
        var today = DateTime.Today;
        var topErrorTasks = await _quartzTaskLogRep.AsQueryable()
            .InnerJoin<QuartzTaskEntity>((a, b) => a.TaskId == b.Id)
            .Where(a => a.BeginDate >= today && a.JobStatus == 1) // 失败任务
            .GroupBy((a, b) => b.TaskName)
            .Select((a, b) => new
            {
                TaskName = b.TaskName,
                ErrorCount = SqlFunc.AggregateAvg(a.Id)
            })
            .OrderByDescending(x => x.ErrorCount)
            .Take(10)
            .ToListAsync();
        var list = topErrorTasks.Adapt<List<dynamic>>();
        return list;
    }

    [HttpGet]
    public async Task<List<dynamic>> GetFailureRate()
    {
        var query = await _quartzTaskLogRep.AsQueryable()
                        .InnerJoin<QuartzTaskEntity>((a, b) => a.TaskId == b.Id)
                         .Where((a, b) => a.BeginDate.HasValue)
                         .GroupBy((a, b) => b.TaskName)
                         .Select((a, b) => new
                         {
                             TaskName = b.TaskName,
                             LastExec = SqlFunc.AggregateMax(a.BeginDate), // 最近一次执行时间
                             Total = SqlFunc.AggregateCount(a.Id),
                             Fail = SqlFunc.AggregateCount(a.JobStatus != 0)
                         })
                         .OrderByDescending(x => x.LastExec) // 按最近执行排序
                         .Take(10)                           // 只取前 10 个
                         .ToListAsync();                    // 转内存计算失败率
        var result = query
                     .Select(g => new
                     {
                         g.TaskName,
                         FailRate = g.Total == 0 ? 0 : Math.Round((double)g.Fail / g.Total * 100, 2)
                     })
                     .ToList<dynamic>();
        return result;
    }

    [HttpGet]
    public async Task<List<dynamic>> GetTrendInfo()
    {
        var query = await _quartzTaskLogRep.AsQueryable()
                        .Where(x => x.BeginDate.HasValue && x.BeginDate.Value.Date == DateTime.Today)
                        .GroupBy(x => x.BeginDate.Value.Hour)
                        .Select(g => new
                        {
                            Hour = g.BeginDate.Value.Hour, // 小时 (0-23)
                            SuccessCount = SqlFunc.AggregateCount(g.JobStatus == 0),
                            FailCount = SqlFunc.AggregateCount(g.JobStatus != 0),
                            AvgDuration = SqlFunc.AggregateCount(g.DurationMs)
                        })
                        .OrderBy(x => x.Hour)
                        .ToListAsync();

        var list = query.Adapt<List<dynamic>>();
        return list;
    }

    [NonAction]
    public async Task<bool> AddLogAsync(QuartzTaskLogEntity input)
    {
        //var entity = Mapper.Map<QuartzTaskLogEntity>(input);
        var id = await _quartzTaskLogRep.InsertReturnSnowflakeIdAsync(input);
        return id > 0;
    }

    /// <summary>
    /// 暂停作业
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> PauseAsync(QuartzTaskEntity entity)
    {
        QuartzTaskEntity task = null;
        var jobExists = await IsQuartzJob(entity.TaskName, entity.GroupName);

        if (jobExists.Item1)
        {
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(entity.GroupName)).Result.ToList();
            JobKey jobKey = jobKeys.FirstOrDefault(s => scheduler.GetTriggersOfJob(s).Result.Any(x =>
            {
                if ((x is CronTriggerImpl cti) && cti.Name == entity.TaskName)
                {
                    return true;
                }
                else if ((x is SimpleTriggerImpl sti) && sti.Name == entity.TaskName)
                {
                    return true;
                }
                return false;
            }));
            var triggers = await scheduler.GetTriggersOfJob(jobKey);
            ITrigger trigger = triggers?.Where(x =>
            {
                if ((x is CronTriggerImpl cti) && cti.Name == entity.TaskName)
                {
                    return true;
                }
                else if ((x is SimpleTriggerImpl sti) && sti.Name == entity.TaskName)
                {
                    return true;
                }
                return false;
            }).FirstOrDefault();
            await scheduler.PauseTrigger(trigger.Key);
        }

        task = await _quartzTaskRep.AsQueryable().Where(a => a.TaskName == entity.TaskName && a.GroupName == entity.GroupName).FirstAsync();
        if (task != null)
        {
            task.Status = (int)JobStateEnum.Pause;
            await _quartzTaskRep.AsUpdateable(task).ExecuteCommandAsync();
        }
        else
        {
            return false;
        }

        return true;
    }
    /// <summary>
    /// 启动作业
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> StartAsync(QuartzTaskEntity input)
    {
        var jobExists = await IsQuartzJob(input.TaskName, input.GroupName);
        var task = await _quartzTaskRep.AsQueryable().Where(a => a.Id == input.Id).FirstAsync();
        task.Status = (int)JobStateEnum.Start;
        IScheduler scheduler = await _schedulerFactory.GetScheduler();
        if (!jobExists.Item1) //如果不存在则加入
        {
            IJobDetail job = null;
            if (task.TaskType == 2)
            {
                job = JobBuilder.Create<ClassLibraryJob>()
                .WithIdentity(task.TaskName, task.GroupName)
                .WithDescription(task.Id.ToString())
                .Build();
            }
            else
            {
                job = JobBuilder.Create<HttpResultfulJob>()
                .WithIdentity(task.TaskName, task.GroupName)
                .WithDescription(task.Id.ToString())
                .Build();
            }
            var builder = TriggerBuilder.Create()
                  .WithIdentity(task.TaskName, task.GroupName)
                  .WithDescription(task.Describe);

            if (task.TriggerType == TriggerTypeEnum.Simple)
            {
                builder = builder.WithSimpleSchedule((b) =>
                {
                    b.WithIntervalInSeconds(int.Parse(task.Interval));
                });
            }
            else
            {
                builder = builder.WithCronSchedule(task.Interval);
            }

            ITrigger trigger = builder.Build();
            if (_jobFactory != null)
            {
                scheduler.JobFactory = _jobFactory;
            }
            await scheduler.ScheduleJob(job, trigger);
            await scheduler.Start();
        }
        else //存在则直接启动
        {
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(task.GroupName)).Result.ToList();
            JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result.Any(x =>
            {
                if ((x is CronTriggerImpl cti) && cti.Name == task.TaskName)
                {
                    return true;
                }
                else if ((x is SimpleTriggerImpl sti) && sti.Name == task.TaskName)
                {
                    return true;
                }
                return false;
            })).FirstOrDefault();
            var triggers = await scheduler.GetTriggersOfJob(jobKey);
            ITrigger trigger = triggers?.Where(x =>
            {
                if (x is CronTriggerImpl cti && cti.Name == task.TaskName)
                {
                    return true;
                }
                else if (x is SimpleTriggerImpl sti && sti.Name == task.TaskName)
                {
                    return true;
                }
                return false;
            }).FirstOrDefault();
            //await scheduler.ResumeTrigger(trigger.Key);
            await scheduler.ResumeJob(jobKey);
        }
        await _quartzTaskRep.AsUpdateable(task).ExecuteCommandAsync();
        return true;
    }

    /// <summary>
    /// 立即执行一次作业
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> RunAsync(QuartzTaskEntity input)
    {
        var jobExists = await IsQuartzJob(input.TaskName, input.GroupName);
        if (jobExists.Item1)
        {
            var task = await _quartzTaskRep.AsQueryable().Where(a => a.Id == input.Id).FirstAsync();
            //taskmodle.Status = (int)JobState.立即执行;
            IScheduler scheduler = await _schedulerFactory.GetScheduler();
            List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(task.GroupName)).Result.ToList();
            JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result.Any(x =>
            {
                if ((x is CronTriggerImpl cti) && cti.Name == task.TaskName)
                {
                    return true;
                }
                else if ((x is SimpleTriggerImpl sti) && sti.Name == task.TaskName)
                {
                    return true;
                }
                return false;
            })).FirstOrDefault();
            var triggers = await scheduler.GetTriggersOfJob(jobKey);
            ITrigger trigger = triggers?.Where(x =>
            {
                if (x is CronTriggerImpl cti && cti.Name == task.TaskName)
                {
                    return true;
                }
                else if (x is SimpleTriggerImpl sti && sti.Name == task.TaskName)
                {
                    return true;
                }
                return false;
            }).FirstOrDefault();
            await scheduler.TriggerJob(jobKey);
            return true;
        }
        else
        {
            throw ResultOutput.Exception(jobExists.Item2);
        }
    }

    /// <summary>
    /// 获取日志
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<PageOutput<QuartzTaskLogDto>> GetLogPageAsync(PageInput<long> input)
    {
        var filter = input.Filter;
        var list = await _quartzTaskLogRep.AsQueryable().Where(v => v.TaskId == filter)
             .OrderByDescending(v => v.ProTime)
             .Select<QuartzTaskLogDto>()
             .ToPagedListAsync(input.CurrentPage, input.PageSize);

        var data = new PageOutput<QuartzTaskLogDto>()
        {
            List = list.Items.ToList(),
            Total = list.Total
        };
        return data;
    }
    /// <summary>
    /// 判断是否存在此任务
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="groupName"></param>
    /// <returns></returns>
    [NonAction]
    public async Task<(bool, string)> IsQuartzJob(string taskName, string groupName)
    {
        IScheduler scheduler = await _schedulerFactory.GetScheduler();
        List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)).Result.ToList();
        if (jobKeys == null || jobKeys.Count() == 0)
        {
            return (false, $"未找到分组[{groupName}]");
        }
        //JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result.Any(x => (x as CronTriggerImpl).Name == taskName)).FirstOrDefault();
        JobKey jobKey = jobKeys.FindAll((s) =>
        {
            var res = scheduler.GetTriggersOfJob(s).Result;
            foreach (var item in res)
            {
                if (item is CronTriggerImpl cti && cti.Name == taskName)
                {
                    return true;
                }
                else if (item is SimpleTriggerImpl sti && sti.Name == taskName)
                {
                    return true;
                }
            }
            return false;
        }).FirstOrDefault();
        if (jobKey == null)
        {
            return (false, $"未找到任务{taskName}]");
        }
        var triggers = await scheduler.GetTriggersOfJob(jobKey);
        ITrigger trigger = triggers?.Where(x =>
        {
            //(x as CronTriggerImpl).Name == taskName
            if (x is CronTriggerImpl cti && cti.Name == taskName)
            {
                return true;
            }
            else if (x is SimpleTriggerImpl sti && sti.Name == taskName)
            {
                return true;
            }
            return false;
        }).FirstOrDefault();

        if (trigger == null)
        {
            return (false, $"未找到触发器[{taskName}]");
        }
        return (true, "");
    }


    [NonAction]
    public bool IsValidExpression(string cronExpression)
    {
        CronTriggerImpl trigger = new CronTriggerImpl();
        trigger.CronExpressionString = cronExpression;
        DateTimeOffset? date = trigger.ComputeFirstFireTimeUtc(null);
        var iscron = date != null;
        if (!iscron)
            return false;
        return true;
    }
    private IJobDetail CreateJob(QuartzTaskEntity entity)
    {
        IJobDetail job = null;
        if (entity.TaskType == 2)
        {
            job = JobBuilder.Create<ClassLibraryJob>()
            .WithIdentity(entity.TaskName, entity.GroupName)
            .WithDescription(entity.Id.ToString())
            .Build();
        }
        else
        {
            job = JobBuilder.Create<HttpResultfulJob>()
            .WithIdentity(entity.TaskName, entity.GroupName)
            .WithDescription(entity.Id.ToString())
            .Build();
        }
        return job;
    }
    private ITrigger CreateTrigger(QuartzTaskEntity entity)
    {
        var builder = TriggerBuilder.Create()
                  .WithIdentity(entity.TaskName, entity.GroupName)
                  .WithDescription(entity.Id.ToString())
                  .WithDescription(entity.Describe);

        if (entity.TriggerType == TriggerTypeEnum.Simple)
        {
            builder = builder.WithSimpleSchedule((b) =>
            {
                b.WithIntervalInSeconds(int.Parse(entity.Interval));
                b.WithRepeatCount(-1);
                b.WithMisfireHandlingInstructionFireNow();
            });
        }
        else
        {
            builder = builder.WithCronSchedule(entity.Interval, cronScheduleBuilder => cronScheduleBuilder.WithMisfireHandlingInstructionFireAndProceed());
        }
        ITrigger trigger = builder.Build();
        return trigger;
    }
}
