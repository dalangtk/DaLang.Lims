using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DaLang.Lims.Tools.Handlers;
using NewLife.Http;
using NewLife.Log;
using System.Windows;

namespace DaLang.Lims.Tools.ViewModels;

public partial class MainWindowViewModel : ObservableObject
{
    private HttpServer _server;
    private TextFileLog _log;

    [ObservableProperty]
    private bool _canStart = true;

    [ObservableProperty]
    private bool _canStop = false;

    [ObservableProperty]
    private string _logText = string.Empty;

    public MainWindowViewModel()
    {
        _log = TextFileLog.Create("");
        _server = new HttpServer
        {
            Port = 8080,
            Log = XTrace.Log,
            SessionLog = XTrace.Log
        };
        _server.Map("/ws", new LimsHandler { LogAction = WriteLog });
        StartService();
    }

    private void WriteLog(string logStr)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            LogText += DateTime.Now + "\t" + logStr + Environment.NewLine;
        });
    }

    [RelayCommand]
    public void StartService()
    {
        try
        {
            _server.Start();
            _log.Info("server start.");
            WriteLog("server start.");
        }
        catch (Exception ex)
        {
            _log.Error("StartService error:" + ex.Message);
            WriteLog("StartService error:" + ex.Message);
            MessageBox.Show(ex.Message);
            return;
        }
        CanStart = false;
        CanStop = true;
    }
    [RelayCommand]
    public void StopService()
    {
        try
        {
            _server.Stop("user close.");
        }
        catch (Exception ex)
        {
            _log.Error("StopService error:" + ex.Message);
            WriteLog("StopService error:" + ex.Message);
            MessageBox.Show(ex.Message);
            return;
        }
        CanStart = true;
        CanStop = false;
    }
}
