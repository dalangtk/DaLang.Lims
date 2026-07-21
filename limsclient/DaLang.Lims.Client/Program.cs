using NetDimension.NanUI;

namespace DaLang.Lims.Client;

internal static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main(string[] args)
    {
        var builder = NanUIApp.CreateBuilder();

        var app = builder
            // 使用WinFormiumStartup的子类来启动应用程序，这个子类必须继承自WinFormiumStartup类，这个类提供了一些方法来配置应用程序。
            .UseNanUIApp<StartApp>()
            // 启用内部浏览器，这个版本默认外部Url打开方式是调用系统默认浏览器，需要手动开启内部浏览器后才能使用内部浏览器打开外部Url。
            //.UseEmbeddedBrowser()
            // 演示单例模式，如果你的应用需要单例模式，那么可以使用这个方法来启用单例模式。新版本可以使用ActiveRunningInstance方法来激活已经运行实例的主窗体。
            .UseSingleApplicationInstanceMode(handler =>
            {
                var retval = MessageBox.Show($"已经有一个实例在运行了:{handler.ProcessId}。\r\n是否打开其主窗体？", "单例模式已启用", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (retval == DialogResult.Yes)
                {
                    handler.ActiveRunningInstance();
                }
            })
            // 在这里指定culture字符传来模拟多语言环境
            //.UseCulture("en-US")
            // 是否启用开发者工具菜单，这个菜单可以在主窗体的右键菜单中找到。
            //.UseDevToolsMenu()
            .Build();

        app.Run();
    }
}