using DaLang.Lims.Client.Handler.Dto;
using DaLang.Lims.Client.Handler.Util;
using DaLang.Lims.Client.Handler.Utils;
using Sunny.UI;

namespace DaLang.Lims.Client.Handler.Camera;

public partial class FrmCamera : UIForm
{
    private ICameraDevice _device;
    private SnapShotDto _snapInput;
    public Action<ResultDto> SnapShotAction;

    public FrmCamera(SnapShotDto input)
    {
        InitializeComponent();

        //Icon = Resource.icon;
        _snapInput = input;
        this.Text = input.SampleNo;
        this.TopMost = true;

        Init();
        InitEvent();
    }

    public void ChangeExam(SnapShotDto input)
    {
        _snapInput = input;
        this.BeginInvoke(() =>
        {
            this.Text = input.SampleNo;
        });
    }

    private void InitEvent()
    {
        cb_device.SelectedIndexChanged += Cb_device_SelectedIndexChanged;
        this.SizeChanged += FrmCamera_SizeChanged;
        panel_shot.MouseClick += Panel_shot_MouseClick;
        this.FormClosing += FrmCamera_FormClosing;
    }

    private void FrmCamera_FormClosing(object? sender, FormClosingEventArgs e)
    {
        _device.StopCamera();
    }

    private async void Panel_shot_MouseClick(object? sender, MouseEventArgs e)
    {
        var ret = new ResultDto { Success = true, Component = _snapInput.Component };
        var baseUrl = AppSettings.Configuration["LimsServiceUrl"];

        var fileName = $"{_snapInput.SampleNo}_{DateTime.Now.ToString("yyyyMMddHHmmss")}.jpg";

        _device.Snap();
        _device.SnapAction = async (buff) =>
        {
            try
            {
                var ms = new MemoryStream(buff);

                var r = await CommonHelper.UploadFile(ms, fileName, $"{baseUrl}/api/exam/exam-images/upload-exam-image", _snapInput.ExamInfoId, _snapInput.IsGrossExamination, _snapInput.AccessToken);

                var result = JsonHelper.Deserialize<ResultDto>(r);
                if (!result.Success)
                {
                    ret.Success = false;
                    ret.Msg = $"{_snapInput.SampleNo}上传图片失败！{result.Msg}";
                }
                else
                {
                    ret.Success = true;
                    ret.Msg = $"{_snapInput.SampleNo}上传图片成功.";
                }
            }
            catch (Exception ex)
            {

            }
            SnapShotAction?.Invoke(ret);
        };
    }

    private void FrmCamera_SizeChanged(object? sender, EventArgs e)
    {
        _device.SizeChange();
    }

    private void Cb_device_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (cb_device.Items.Count == 0)
            return;

        var deviceName = cb_device.SelectedItem?.ToString();
        if (string.IsNullOrWhiteSpace(deviceName))
            return;
        if (_device.CurrentDeviceName.Equals(deviceName))
            return;
        _device.ChangeCamera(deviceName);
    }

    private void Init()
    {
        _device = new DirectShowCamera(pictureBox1);
        var deviceList = _device.GetDevices();
        if (deviceList.Any())
        {
            foreach (var item in deviceList)
            {
                cb_device.Items.Add(item);
            }
        }
        cb_device.SelectedIndex = 0;

        _device.StartCamera(deviceList[0]);
    }
}
