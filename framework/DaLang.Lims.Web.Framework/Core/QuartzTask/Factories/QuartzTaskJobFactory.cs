using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;

namespace DaLang.Lims.Web.Framework.Core.QuartzTask.Factories;

public class QuartzTaskJobFactory : IJobFactory
{
    private static IServiceScopeFactory _serviceProvider;
    public QuartzTaskJobFactory(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceProvider = serviceScopeFactory;
    }
    public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
    {
        var sevice = _serviceProvider.CreateScope();
        return sevice.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
    }

    public void ReturnJob(IJob job)
    {
        (job as IDisposable)?.Dispose();
    }
}
