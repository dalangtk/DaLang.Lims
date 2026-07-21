using DaLang.Lims.Client.Handler;
using DaLang.Lims.Client.Handler.Util;
using NetDimension.NanUI;
using NetDimension.NanUI.Forms;

namespace DaLang.Lims.Client;

internal class MainWindow : Formium
{
    private LimsHandler handler;
    protected override bool DisableAboutMenu => true;
    public MainWindow()
    {
        //_log = TextFileLog.Create("");
    }
    public MainWindow(ChromiumEnvironment env)
    {
        Url = "http://localhost:8100/";
        Icon = Resource.icon;
        Loaded += MyWindow_Loaded;
        PageLoadEnd += MyWindow_PageLoadEnd;
        Closing += MyWindow_Closing;
    }

    protected override void OnKeyEvent(KeyEventEventArgs args)
    {
        if (args.KeyEvent.WindowsKeyCode == (int)Keys.F12)
            ShowDevTools();

        base.OnKeyEvent(args);
    }

    private void MyWindow_Closing(object? sender, ClosingEventArgs e)
    {
        if (MessageBox.Show(this, "确定关闭系统？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            e.Cancel = true;
    }

    private void MyWindow_Loaded(object? sender, BrowserEventArgs e)
    {
        handler = new(this);
        RegisterJavaScriptRequestHandler("PrintLabel", handler.PrintLabelHandler);
        RegisterJavaScriptRequestHandler("SnapShot", handler.SnapShot);
        RegisterJavaScriptRequestHandler("ChangeExam", handler.ChangeExam);
    }

    private void MyWindow_PageLoadEnd(object? sender, PageLoadEndEventArgs e)
    {
    }

    protected override FormStyle ConfigureWindowStyle(WindowStyleBuilder builder)
    {
        var style = builder.UseSystemForm();

        style.WindowState = FormiumWindowState.Maximized;

        style.DefaultAppTitle = AppSettings.Configuration["AppName"] ?? "DaLang";

        style.StartCentered = StartCenteredMode.CenterScreen;

        // 移除系统窗体的标题栏
        style.TitleBar = true;
        style.BackdropType = SystemFormBackdropType.Mica;

        // 指定系统深浅色主题模式，默认将自动检测当前系统的深浅色主题模式。也可以手动指定
        //style.ColorMode = FormiumColorMode.Dark;

        // 是否启用网页的页面标题
        style.UsePageTitle = true;

        //style.StartCentered = true;

        // 是否启用Kiosk模式，启用后将禁用任务栏
        //var style = builder.UseKisokForm();
        //style.DisableTaskBar = true;

        return style;
    }
}