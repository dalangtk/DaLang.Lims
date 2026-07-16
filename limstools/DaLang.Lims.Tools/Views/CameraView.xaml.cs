using DaLang.Lims.Tools.Util;
using DaLang.Lims.Web.Common.Helpers;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using WPFMediaKit.DirectShow.Controls;

namespace DaLang.Lims.Tools.Views;

/// <summary>
/// CameraView.xaml 的交互逻辑
/// </summary>
public partial class CameraView : Window
{
    private string _sampleNo;
    private long _examInfoId;
    private bool _isGrossExamination = false;
    private string _accessToken;
    public Action<bool, string, string> SnapShotAction;
    public Action CamrraClosedAction;
    private string _component;

    public CameraView()
    {
        InitializeComponent();

        this.Closed += CameraView_Closed;
    }

    private void CameraView_Closed(object? sender, EventArgs e)
    {
        CamrraClosedAction?.Invoke();
    }

    public CameraView(long examInfoId, bool isGrossExamination, string sampleNo, string accessToken, string component) : this()
    {
        _examInfoId = examInfoId;
        _isGrossExamination = isGrossExamination;
        _accessToken = accessToken;
        _sampleNo = sampleNo;
        _component = component;
        this.Title = sampleNo;
    }
    ~CameraView()
    {
        vce = null;
    }
    public void SetExamInfo(long examInfoId, bool isGrossExamination, string sampleNo, string accessToken, string component)
    {
        _examInfoId = examInfoId;
        _isGrossExamination = isGrossExamination;
        _accessToken = accessToken;
        _sampleNo = sampleNo;
        _component = component;
        Application.Current.Dispatcher.Invoke(() =>
        {
            this.Title = sampleNo;
        });
    }
    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        cb.ItemsSource = MultimediaUtil.VideoInputNames;
        if (MultimediaUtil.VideoInputNames.Length > 0)
            cb.SelectedIndex = 0;
        else
            SnapShotAction?.Invoke(false, "未连接摄像头！", _component);
    }

    private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        vce.VideoCaptureSource = (string)cb.SelectedItem;
    }

    public async void Button_Click(object sender, RoutedEventArgs e)
    {
        if (_examInfoId <= 0)
            return;

        var bmp = new RenderTargetBitmap((int)vce.ActualWidth, (int)vce.ActualHeight, 96, 96, PixelFormats.Default);
        bmp.Render(vce);
        var encoder = new JpegBitmapEncoder();
        encoder.Frames.Add(BitmapFrame.Create(bmp));
        using (MemoryStream ms = new MemoryStream())
        {
            encoder.Save(ms);
            byte[] captureData = ms.ToArray();
            File.WriteAllBytes("E:/1.jpg", captureData);

            var ret = await CommonHelper.UploadFile("E:/1.jpg", "http://localhost:8000/api/exam/exam-images/upload-exam-image", _examInfoId, _isGrossExamination, _accessToken);

            var result = JsonHelper.Deserialize<MessageResult>(ret);
            if (!result.Success)
            {
                SnapShotAction?.Invoke(false, $"上传图片失败！{result.Msg}", _component);
            }
            else
            {
                SnapShotAction?.Invoke(true, $"{_sampleNo}上传图片成功.", _component);
            }
        }
    }
}
