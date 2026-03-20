using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using SqlSugar;
using System;
using System.Linq;
using System.Reflection;
using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Auth;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.Logs;
using DaLang.Lims.Web.Framework.Repositories;
using DaLang.Lims.Web.Framework.Resources;
using DaLang.Lims.Web.Common.Helpers;

namespace DaLang.Lims.Web.Framework.Core.RegisterModules
{
    public static class DefaultInject
    {
        public static void RegisterAssemblies(this IServiceCollection services, AppConfig appConfig)
        {
            Assembly[] assemblies = AssemblyHelper.GetAssemblyList(appConfig.AssemblyNames);
            services.AddScoped(typeof(Lazy<>));//注册Lazy
            foreach (var item in assemblies)
            {
                var tepes = item.GetExportedTypes();
                foreach (var t in tepes)
                {
                    var interfaces = t.GetInterfaces()?.ToList();
                    if (t.GetCustomAttribute<SingleInstanceAttribute>(false) != null
                        || t.GetCustomAttribute<InjectSingletonAttribute>(false) != null)
                    {
                        var idx = interfaces.FindIndex(o => o.Name.StartsWith("I"));
                        if (idx > -1)
                            services.AddSingleton(interfaces[idx], t);
                        services.AddSingleton(t);
                    }
                }
            }

            static bool Predicate(Type a) => !a.IsDefined(typeof(NonRegisterIOCAttribute), true)
                && (a.Name.EndsWith("Service") || a.Name.EndsWith("Repository") || typeof(IRegisterIOC).IsAssignableFrom(a))
                && !a.IsAbstract && !a.IsInterface && a.IsPublic;

            foreach (var item in assemblies)
            {
                var types = item.GetExportedTypes();
                types.Where(Predicate).ToList().ForEach(c =>
                {
                    var interfaces = c.GetInterfaces()?.ToList();
                    if (interfaces.FindIndex(o => o.Name.Contains("I" + c.Name)) > -1)
                    {
                        var idx = interfaces.FindIndex(o => o.Name.Contains("I" + c.Name));
                        services.AddScoped(interfaces[idx], c);
                    }
                    services.AddScoped(c);
                });

            }

            services.AddScoped(typeof(User));
            services.AddScoped(typeof(ApiHelper));
            services.AddScoped(typeof(AdminRepositoryBase<>));

            services.AddTransient(typeof(IUser), typeof(User));
            services.AddScoped(typeof(ILogHandler), typeof(LogHandler));
            services.AddScoped(typeof(IStringLocalizer), typeof(AdminLocalizer));
            services.AddScoped(typeof(IUserToken), typeof(UserToken));
            services.AddScoped(typeof(IPasswordHasher<>), typeof(PasswordHasher<>));
        }
    }
}
