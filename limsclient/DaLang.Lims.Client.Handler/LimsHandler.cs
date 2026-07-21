using DaLang.Lims.Client.Handler.Camera;
using DaLang.Lims.Client.Handler.Dto;
using DaLang.Lims.Client.Handler.Extensions;
using DaLang.Lims.Client.Handler.Util;
using DaLang.Lims.Client.Handler.Utils;
using NetDimension.NanUI;
using NetDimension.NanUI.JavaScript;

namespace DaLang.Lims.Client.Handler
{
    public class LimsHandler
    {
        private Formium _hostWindow;
        private BarTenderExtension _bartenderHelper;
        private FrmCamera _cameraFrm;
        public LimsHandler(Formium hostWindow)
        {
            _hostWindow = hostWindow;
        }
        public async void PrintLabelHandler(JavaScriptValue args, JavaScriptPromise promise)
        {
            var result = new ResultDto { Success = true };
            try
            {
                string json = args;
                var data = JsonHelper.Deserialize<PrintLabelDto>(json);

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
            catch (Exception ex)
            {
                result.Success = false;
                result.Msg = ex.Message;
                //_log.Error("PrintLabelError:" + ex.Message);
            }
            promise.Resolve(JsonHelper.Serialize(result));
        }

        public async void SnapShot(JavaScriptValue args, JavaScriptPromise promise)
        {
            var result = new ResultDto { Success = true };
            try
            {
                string json = args;
                var data = JsonHelper.Deserialize<SnapShotDto>(json);

                _hostWindow.InvokeOnUIThread(() =>
                {
                    if (_cameraFrm != null)
                    {
                        _cameraFrm.Close();
                        _cameraFrm = null;
                    }
                    _cameraFrm = new FrmCamera(data)
                    {
                        SnapShotAction = OnSnapShot
                    };
                    _cameraFrm.Show();
                });
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Msg = ex.Message;
                //_log.Error("SnapShot:" + ex.Message);
            }
            promise.Resolve(JsonHelper.Serialize(result));
        }

        public async void ChangeExam(JavaScriptValue args, JavaScriptPromise promise)
        {
            var result = new ResultDto { Success = true };
            try
            {
                string json = args;
                var data = JsonHelper.Deserialize<SnapShotDto>(json);

                _cameraFrm?.ChangeExam(data);
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Msg = ex.Message;
            }
            promise.Resolve(JsonHelper.Serialize(result));
        }

        private void OnSnapShot(ResultDto ret)
        {
            _hostWindow.PostJavaScriptMessage("ImageMessageHandler" + ret.Component, JsonHelper.Serialize(ret));
        }
    }
}
