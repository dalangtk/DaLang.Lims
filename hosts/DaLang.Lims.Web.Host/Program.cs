using AgileConfig.Client;
using DaLang.Lims.Agent.Extension;
using DaLang.Lims.Web.ApiUI;
using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.Framework.Core;
using DaLang.Lims.Web.Framework.Core.Configs;
using DaLang.Lims.Web.Framework.Core.QuartzTask.Extensions;
using DaLang.Lims.Web.Framework.Core.Startup;
using Savorboard.CAP.InMemoryMessageQueue;
using System.Reflection;


new HostApp(new HostAppOptions
{
    ConfigurePreServices = context =>
    {
    },
    ConfigurePreWebApplicationBuilder = builder =>
    {
        var configuration = builder.Configuration;
        if (bool.Parse(configuration["UseAgileConfig"] ?? "false"))
        {
            string env = "DEV";
            if (builder.Environment != null)
            {
                Console.WriteLine($"builder.Environment.EnvironmentName:{builder.Environment.EnvironmentName}");
                switch (builder.Environment.EnvironmentName)
                {
                    case "Development":
                        env = "DEV";
                        break;
                    case "Production":
                        env = "PROD";
                        break;
                    case "Staging":
                        env = "STAGING";
                        break;
                    default:
                        env = "DEV";
                        break;
                }
            }
            var appId = configuration["AgileConfig:appId"];
            var secret = configuration["AgileConfig:secret"];
            var nodes = configuration["AgileConfig:nodes"];
            var client = new ConfigClient(appId, secret, nodes, env);
            builder.Host.UseAgileConfig(client);
        }
    },
    //配置后置服务
    ConfigurePostServices = context =>
    {
        //添加cap事件总线
        var appConfig = AppInfo.GetRequiredService<AppConfig>(false);
        Assembly[] assemblies = AssemblyHelper.GetAssemblyList(appConfig.AssemblyNames);

        //var dbConfig = AppInfo.GetRequiredService<DbConfig>(false);
        //var rabbitMQ = context.Configuration.GetSection("CAP:RabbitMq").Get<RabbitMQOptions>();
        context.Services.AddCap(config =>
        {
            config.UseInMemoryStorage();
            config.UseInMemoryMessageQueue();

            //<PackageReference Include="DotNetCore.CAP.MySql" Version="7.1.1" />
            //<PackageReference Include="DotNetCore.CAP.RabbitMQ" Version="7.1.1" />

            //config.UseMySql(dbConfig.ConnectionString);
            //config.UseRabbitMQ(mqConfig => {
            //    mqConfig.HostName = rabbitMQ.HostName;
            //    mqConfig.Port = rabbitMQ.Port;
            //    mqConfig.UserName = rabbitMQ.UserName;
            //    mqConfig.Password = rabbitMQ.Password;
            //    mqConfig.ExchangeName = rabbitMQ.ExchangeName;
            //});
            config.UseDashboard(dashoptions =>
            {
                dashoptions.PathMatch = "/cap";  //面板地址
            });
        }).AddSubscriberAssembly(assemblies);

        var configuration = context.Configuration;
        context.Services.AddAgent(configuration);
    },

    //配置Autofac容器
    ConfigureAutofacContainer = (builder, context) =>
    {

    },

    //配置Mvc
    ConfigureMvcBuilder = (builder, context) =>
    {
    },

    //配置后置中间件
    ConfigurePostMiddleware = context =>
    {
        var app = context.App;
        var env = app.Environment;
        var appConfig = app.Services.GetService<AppConfig>();

        #region 新版Api文档
        if (env.IsDevelopment() || appConfig.ApiUI.Enable)
        {
            app.UseApiUI(options =>
            {
                options.RoutePrefix = appConfig.ApiUI.RoutePrefix;
                appConfig.Swagger.Projects?.ForEach(project =>
                {
                    options.SwaggerEndpoint($"/{options.RoutePrefix}/swagger/{project.Code.ToLower()}/swagger.json", project.Name);
                });
            });
        }
        #endregion

    },

    ConfigureSwaggerUI = options =>
    {
        //options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.Full);
    },
    OnApplicationStarted = context =>
    {
        var app = context.App;
        app.UseAgent();
        app.UseQuartz();
    }
}).Run(args, typeof(Program).Assembly);

