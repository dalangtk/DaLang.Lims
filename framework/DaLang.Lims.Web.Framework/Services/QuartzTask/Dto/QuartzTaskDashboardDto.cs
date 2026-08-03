namespace DaLang.Lims.Web.Framework.Services.QuartzTask.Dto;

public class QuartzTaskDashboardDto
{
    /// <summary>
    /// 任务执行次数
    /// </summary>
    public int JobCounts { get; set; }
    /// <summary>
    /// 错误率
    /// </summary>
    public double ErrorCounts { get; set; }

    /// <summary>
    /// 平均耗时
    /// </summary>
    public double AverageTime { get; set; }

    /// <summary>
    /// 运行任务/任务总数
    /// </summary>
    public string RunJobs { get; set; }
}
