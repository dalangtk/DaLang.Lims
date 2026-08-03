using DaLang.Lims.Web.Framework.Services.QuartzTask;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DaLang.Lims.Web.Framework.Core.QuartzTask;

public class QuartzStartupService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public QuartzStartupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
       // Task.Run(async () =>
       //{
       //    while (true)
       //    {
       //        try
       //        {
       //            await Task.Delay(5000);
       //            using var scope = _scopeFactory.CreateScope();
       //            var task = scope.ServiceProvider.GetService<IQuartzTaskService>();

       //            task.InitJobs().GetAwaiter().GetResult();
       //            break;
       //        }
       //        catch (Exception)
       //        {
       //        }
       //    }

       //});
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
