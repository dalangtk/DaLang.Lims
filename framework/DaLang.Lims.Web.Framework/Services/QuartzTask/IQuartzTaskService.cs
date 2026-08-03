using DaLang.Lims.Web.Framework.Contracts.SysQuartzTaskLog.Dto;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Framework.Domain.SysQuartzTask;
using DaLang.Lims.Web.Framework.Domain.SysQuartzTaskLog;
using DaLang.Lims.Web.Framework.Services.QuartzTask.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Services.QuartzTask;

public interface IQuartzTaskService
{
    /// <summary>
    /// 获取作业
    /// </summary>
    /// <returns></returns>
    Task<QuartzTaskDto> GetAsync(long id);

    /// <summary>
    /// 获取所有作业
    /// </summary>
    /// <returns></returns>
    Task<List<QuartzTaskDto>> GetListAsync(QuartzTaskQueryInput input);

    /// <summary>
    /// 添加任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> AddAsync(QuartzTaskAddInput input);

    /// <summary>
    /// 删除任务
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteAsync(long id);

    /// <summary>
    /// 更新任务
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>

    Task<bool> UpdateAsync(QuartzTaskUpdateInput input);

    /// <summary>
    /// 暂停
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task<bool> PauseAsync(QuartzTaskEntity entity);

    /// <summary>
    /// 启动
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> StartAsync(QuartzTaskEntity input);

    /// <summary>
    /// 立即执行
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<bool> RunAsync(QuartzTaskEntity input);

    /// <summary>
    /// 获取看板数据
    /// </summary>
    /// <returns></returns>
    Task<QuartzTaskDashboardDto> GetDashboardInfo();

    Task<List<dynamic>> GetTrendInfo();

    Task<List<dynamic>> GetFailureRate();

    Task<List<dynamic>> GetErrorTop10();

    Task<dynamic> GetDurationDistribution();

    /// <summary>
    /// 获取单个任务
    /// </summary>
    /// <param name="taskName"></param>
    /// <param name="groupName"></param>
    /// <returns></returns>
    [NonAction]
    Task<QuartzTaskEntity> GetJob(string taskName, string groupName);

    /// <summary>
    /// 初始化任务
    /// </summary>
    /// <returns></returns>
    [NonAction]
    Task InitJobs();

    /// <summary>
    /// 新增日志
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [NonAction]
    Task<bool> AddLogAsync(QuartzTaskLogEntity input);

    /// <summary>
    /// 查询日志
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    Task<PageOutput<QuartzTaskLogDto>> GetLogPageAsync(PageInput<long> input);
}
