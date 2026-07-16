using DaLang.Lims.Tools.Extensions;
using DaLang.Lims.Tools.Util;
using DaLang.Lims.Tools.Views;
using DaLang.Lims.Web.Common.Helpers;
using NewLife.Data;
using NewLife.Http;
using System.IO;
using System.Windows;

namespace DaLang.Lims.Tools.Handlers;

public class LimsHandler : IHttpHandler
{
    public Action<string> LogAction;
    private BarTenderExtension _bartenderHelper;
    private Window? _cameraWindow;
    private IHttpContext _context;

    public virtual void ProcessRequest(IHttpContext context)
    {
        _context = context;
        var ws = context.WebSocket;
        ws!.Handler = ProcessMessage;

        Log(string.Format("WebSocket连接 {0}", context.Connection!.Remote));
    }

    /// <summary>处理消息</summary>
    /// <param name="socket"></param>
    /// <param name="message"></param>
    public virtual void ProcessMessage(WebSocket socket, WebSocketMessage message)
    {
        var result = new MessageResult { Success = true };
        bool sendResult = true;
        try
        {
            var remote = socket!.Context!.Connection!.Remote;
            var msg = message.Payload?.ToStr();
            switch (message.Type)
            {
                case WebSocketMessageType.Text:
                    var data = JsonHelper.Deserialize<MessageData<object>>(msg);
                    if (data.Type == MessageType.Ping)
                        return;
                    Log(string.Format("WebSocket收到[{0}] {1}", message.Type, msg));
                    result.Component = data.Component;
                    sendResult = Pross(data);
                    break;
                case WebSocketMessageType.Close:
                    Log(string.Format("WebSocket关闭[{0}] [{1}] {2}", remote, message.CloseStatus, message.StatusDescription!));
                    break;
                case WebSocketMessageType.Ping:
                case WebSocketMessageType.Pong:
                    Log(string.Format("WebSocket心跳[{0}] {1}", message.Type, msg));
                    break;
                default:
                    Log(string.Format("WebSocket收到[{0}] {1}", message.Type, msg));
                    break;
            }
            socket.Send(JsonHelper.Serialize(result));
        }
        catch (Exception ex)
        {
            result.Success = false;
            result.Msg = ex.Message;
            Log(string.Format("ProcessMessage exception: {0}", ex.Message));
        }
        if (sendResult)
            socket.Send(JsonHelper.Serialize(result));
    }

    private bool Pross(MessageData<object> data)
    {
        var result = false;
        if (data == null || string.IsNullOrWhiteSpace(data.Component))
            return result;

        switch (data.Type)
        {
            case MessageType.Print:
                var printInput = JsonHelper.Deserialize<PrintDto>(data.Data.ToString());
                PrintLabel(printInput);
                result = true;
                break;
            case MessageType.SnapShot:
                var snapInput = JsonHelper.Deserialize<SnapShotDto>(data.Data.ToString());
                ShowCameraWindow(snapInput, data.Component);
                result = false;
                break;
            case MessageType.ChangeExam:
                snapInput = JsonHelper.Deserialize<SnapShotDto>(data.Data.ToString());
                if (snapInput.ExamInfoId <= 0)
                    throw new Exception("invalid exam info id！");
                (_cameraWindow as CameraView)?.SetExamInfo(snapInput.ExamInfoId, snapInput.IsGrossExamination, snapInput.SampleNo, snapInput.AccessToken, data.Component);
                result = false;
                break;
        }
        return result;
    }
    private void ShowCameraWindow(SnapShotDto data, string component)
    {
        if (data.ExamInfoId <= 0)
            throw new Exception("invalid exam info id！");

        Application.Current.Dispatcher.Invoke(() =>
        {
            if (_cameraWindow != null)
            {
                _cameraWindow.Close();
                _cameraWindow = null;
            }
            _cameraWindow = new CameraView(data.ExamInfoId, data.IsGrossExamination, data.SampleNo, data.AccessToken, component)
            {
                Topmost = true,
                SnapShotAction = OnSnapShot,
                CamrraClosedAction = () => _cameraWindow = null
            };
            _cameraWindow.Show();
        });
    }
    private void PrintLabel(PrintDto data)
    {
        if (string.IsNullOrWhiteSpace(data.TemplateName))
            throw new Exception("template name can not null");

        var templateDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates");
        if (!File.Exists(Path.Combine(templateDir, data.TemplateName! + ".btw")))
            throw new Exception($"template {data.TemplateName} does not exist");
        if (!CommonHelper.checkHasInstalledSoftWare("BarTender 10.0")
                    && !CommonHelper.checkHasInstalledSoftWare("BarTender 10.1"))
            throw new Exception("barTender 10 not installed\r\n");

        CommonHelper.WriteObjectToFile(data.PrintList, Path.Combine(templateDir, $"PrintSource.txt"));

        if (_bartenderHelper == null)
            _bartenderHelper = new BarTenderExtension(data.TemplateName);

        _bartenderHelper.Print("", $"data.TemplateName_{DateTime.Now:yyyy-MM-dd HH:mm:ss}");
    }

    private void OnSnapShot(bool success, string msg, string component)
    {
        var result = new MessageResult { Success = success, Msg = msg, Component = component, Type = MessageType.SnapShot };
        Log(string.Format("SnapShotAction: {0}", msg));
        _context.WebSocket.Send(JsonHelper.Serialize(result));
    }
    private void Log(string log)
    {
        LogAction?.Invoke(log);
    }
}
