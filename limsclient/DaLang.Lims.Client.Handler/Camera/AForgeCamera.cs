using AForge.Video.DirectShow;

namespace DaLang.Lims.Client.Handler.Camera
{
    public class AForgeCamera : ICameraDevice
    {
        private FilterInfoCollection videoDevices;//所有摄像设备
        private VideoCaptureDevice videoDevice;//摄像设备
        private VideoCapabilities[] videoCapabilities;//摄像头分辨率
        private string _currentDeviceName;

        public string CurrentDeviceName => _currentDeviceName;

        public Action<byte[]> SnapAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void ChangeCamera(string deviceName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetDevices()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices == null || videoDevices.Count == 0)
            {
                throw new Exception("No device connected");
            }

            List<string> cameraList = new List<string>();
            foreach (FilterInfo item in videoDevices)
                cameraList.Add(item.Name);

            return cameraList;
        }

        public void SizeChange()
        {
            throw new NotImplementedException();
        }

        public void Snap()
        {
            throw new NotImplementedException();
        }

        public void StartCamera(string deviceName)
        {
            //videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);//得到机器所有接入的摄像设备
            //if (videoDevices.Count != 0)
            //{
            //    foreach (FilterInfo device in videoDevices)
            //    {
            //        ricbx_Camera.Items.Add(device.Name);//把摄像设备添加到摄像列表中
            //    }
            //}
            //else
            //{
            //    ricbx_Camera.Items.Add("没有找到摄像头");
            //    return;
            //}
            ////cbx_Camera.SelectedIndex = 0;//默认选择第一个
            //bed_Camera.EditValue = ricbx_Camera.Items[0];
            //DoConnect();
        }

        public void StopCamera()
        {
            throw new NotImplementedException();
        }
    }
}
