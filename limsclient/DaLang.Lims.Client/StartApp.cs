
using Microsoft.Extensions.DependencyInjection;
using NetDimension.NanUI;

namespace DaLang.Lims.Client;

internal class StartApp : AppStartup
{
    protected override MainWindowCreationAction? UseMainWindow(MainWindowOptions opts)
    {
        return opts.UseMainFormium<MainWindow>();
    }

    protected override void ProgramMain(string[] args)
    {
        // 现在把 Main 函数搬到这里来。避免用户搞不清主进程和渲染进程的区别，在 Program.cs 里面写太多代码导致子进程内部出现问题。
#if NETCOREAPP3_1_OR_GREATER
        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
#else
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
#endif
    }


    // CEF 的配置可以在Program.cs里面写，也可以在这里写，在这里写更集中，更简洁。
    protected override void ConfigurationChromiumEmbedded(ChromiumEnvironmentBuiler cef)
    {
        //cef.UseInMemoryUserData();
        cef.ConfigureCommandLineArguments(cl =>
        {
            //cl.AppendArgument("disable-web-security");
            //cl.AppendSwitch("no-proxy-server");
            cl.AppendSwitch("enable-gpu");
            //cl.AppendSwitch("disable-gpu");
        });

        cef.ConfigureDefaultSettings(settings =>
        {
            settings.WindowlessRenderingEnabled = true;
        });

        cef.ConfigureDefaultBrowserSettings(settings =>
        {

        });

    }

    protected override void ConfigureServices(IServiceCollection services)
    {
    }
}
