using DaLang.Lims.Web.Framework.Core.QuartzTask.Factories;
using DaLang.Lims.Web.Framework.Core.QuartzTask.Jobs;
using DaLang.Lims.Web.Framework.Core.QuartzTask.Options;
using DaLang.Lims.Web.Framework.Services.QuartzTask;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DaLang.Lims.Web.Framework.Core.QuartzTask.Extensions;

public static class QuartzTaskExtension
{
    public static IServiceCollection AddQuartzUI(this IServiceCollection services)
    {
        services.AddSingleton(a => { return new QuartzTaskOptions(); });
        services.AddScoped<IQuartzTaskService, QuartzTaskService>();

        services.AddScoped<HttpResultfulJob>();
        services.AddScoped<ClassLibraryJob>();
        services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
        services.AddSingleton<IJobFactory, QuartzTaskJobFactory>();
        return services;
    }

    /// <summary>
    /// 自动注入定时任务类
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddQuartzClassJobs(this IServiceCollection services)
    {
        var baseType = typeof(IJobService);
        var path = AppDomain.CurrentDomain.RelativeSearchPath ?? AppDomain.CurrentDomain.BaseDirectory;
        var referencedAssemblies = Directory.GetFiles(path, "*.dll");
        List<Type> typelist = new List<Type>();
        foreach (var item in referencedAssemblies)
        {
            try
            {
                var assembly = Assembly.LoadFrom(item);
                Type[] ts = assembly.GetTypes();
                typelist.AddRange(ts.ToList());
            }
            catch (Exception)
            {

                continue;
            }
        }
        var types = typelist
        .Where(x => x != baseType && baseType.IsAssignableFrom(x)).ToArray();
        var implementTypes = types.Where(x => x.IsClass).ToArray();
        var interfaceTypes = types.Where(x => x.IsInterface).ToArray();
        foreach (var implementType in implementTypes)
        {
            var interfaceType = implementType.GetInterfaces().First();
            services.AddScoped(interfaceType, implementType);
            services.AddScoped(implementType);
            ClassJobsFactory.ClassJobs.Add(implementType.Name);
            ClassJobsFactory.JobServiceMap[implementType.Name] = implementType;
        }
        return services;
    }
    public static IApplicationBuilder UseQuartz(this IApplicationBuilder builder)
    {
        IServiceProvider services = builder.ApplicationServices;
        using (var serviceScope = services.CreateScope())
        {
            var service = serviceScope.ServiceProvider.GetService<IQuartzTaskService>();
            service.InitJobs().GetAwaiter().GetResult();
        }

        return builder;
    }
}
