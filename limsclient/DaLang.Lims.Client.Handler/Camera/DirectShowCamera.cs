using DirectShowLib;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace DaLang.Lims.Client.Handler.Camera
{
    public class DirectShowCamera : ICameraDevice, ISampleGrabberCB, IDisposable
    {
        private int _previewWidth = 640;
        private int _previewHeight = 480;
        private int _previewStride = 0;
        private int _previewFPS = 30;
        private volatile bool isGrab = false;
        IVideoWindow videoWindow = null;
        IMediaControl mediaControl = null;
        IFilterGraph2 graphBuilder = null;
        ICaptureGraphBuilder2 captureGraphBuilder = null;
        DsROTEntry rot = null;
        DsDevice[] devices;
        PictureBox _pictureBox;
        private string _currentDeviceName;
        private string _fileName;

        public string CurrentDeviceName => _currentDeviceName;

        public Action<byte[]> SnapAction { get; set; }

        public DirectShowCamera(PictureBox pictureBox)
        {
            _pictureBox = pictureBox;
        }

        public List<string> GetDevices()
        {
            devices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (devices != null && devices.Length > 0)
            {
                return devices.Select(v => v.Name).ToList();
            }
            else
            {
                throw new Exception("No USB webcam connected");
            }
        }

        public void StartCamera(string deviceName)
        {
            if (devices.Length == 0)
            {
                throw new Exception("No USB webcam connected");
            }
            else
            {
                _pictureBox.Image = null;

                var device = devices.FirstOrDefault(v => v.Name == deviceName);
                if (device == null)
                {
                    throw new Exception($"There is no device named {deviceName}");
                }
                _currentDeviceName = deviceName;
                CaptureVideo(devices[0]);
            }
        }
        public void CaptureVideo(DsDevice device)
        {
            int hr = 0;
            IBaseFilter sourceFilter = null;
            ISampleGrabber sampleGrabber = null;
            try
            {
                // Get DirectShow interfaces
                GetInterfaces();
                // Attach the filter graph to the capture graph
                hr = this.captureGraphBuilder.SetFiltergraph(this.graphBuilder);
                DsError.ThrowExceptionForHR(hr);
                // Use the system device enumerator and class enumerator to find
                // a video capture/preview device, such as a desktop USB video camera.
                sourceFilter = SelectCaptureDevice(device);
                // Add Capture filter to graph.
                hr = this.graphBuilder.AddFilter(sourceFilter, "Video Capture");
                DsError.ThrowExceptionForHR(hr);
                // Initialize SampleGrabber.
                sampleGrabber = new SampleGrabber() as ISampleGrabber;
                // Configure SampleGrabber. Add preview callback.
                ConfigureSampleGrabber(sampleGrabber);
                // Add SampleGrabber to graph.
                hr = this.graphBuilder.AddFilter(sampleGrabber as IBaseFilter, "Frame Callback");
                DsError.ThrowExceptionForHR(hr);
                // Configure preview settings.
                SetConfigParams(this.captureGraphBuilder, sourceFilter, _previewFPS, _previewWidth, _previewHeight);
                // Render the preview
                hr = this.captureGraphBuilder.RenderStream(PinCategory.Preview, MediaType.Video, sourceFilter, (sampleGrabber as IBaseFilter), null);
                DsError.ThrowExceptionForHR(hr);
                SaveSizeInfo(sampleGrabber);
                // Set video window style and position
                SetupVideoWindow();
                // Add our graph to the running object table, which will allow
                // the GraphEdit application to "spy" on our graph
                rot = new DsROTEntry(this.graphBuilder);
                // Start previewing video data
                hr = this.mediaControl.Run();
                DsError.ThrowExceptionForHR(hr);
            }
            catch (Exception ex)
            {
                MessageBox.Show("An unrecoverable error has occurred." + ex.Message);
            }
            finally
            {
                if (sourceFilter != null)
                {
                    Marshal.ReleaseComObject(sourceFilter);
                    sourceFilter = null;
                }
                if (sampleGrabber != null)
                {
                    Marshal.ReleaseComObject(sampleGrabber);
                    sampleGrabber = null;
                }
            }
        }
        public void GetInterfaces()
        {
            int hr = 0;
            // An exception is thrown if cast fail
            this.graphBuilder = (IFilterGraph2)new FilterGraph();
            this.captureGraphBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
            this.mediaControl = (IMediaControl)this.graphBuilder;
            this.videoWindow = (IVideoWindow)this.graphBuilder;
            DsError.ThrowExceptionForHR(hr);
        }
        public IBaseFilter SelectCaptureDevice(DsDevice device)
        {
            object source = null;
            Guid iid = typeof(IBaseFilter).GUID;
            device.Mon.BindToObject(null, null, ref iid, out source);
            return (IBaseFilter)source;
        }
        private void ConfigureSampleGrabber(ISampleGrabber sampleGrabber)
        {
            AMMediaType media;
            int hr;
            // Set the media type to Video/RBG24
            media = new AMMediaType();
            media.majorType = MediaType.Video;
            media.subType = MediaSubType.RGB24;
            media.formatType = FormatType.VideoInfo;
            hr = sampleGrabber.SetMediaType(media);
            DsError.ThrowExceptionForHR(hr);
            DsUtils.FreeAMMediaType(media);
            media = null;
            hr = sampleGrabber.SetCallback(this, 1);
            DsError.ThrowExceptionForHR(hr);
        }
        private void SetConfigParams(ICaptureGraphBuilder2 capGraph, IBaseFilter capFilter, int iFrameRate, int iWidth, int iHeight)
        {
            int hr;
            object config;
            AMMediaType mediaType;
            // Find the stream config interface
            hr = capGraph.FindInterface(
                PinCategory.Capture, MediaType.Video, capFilter, typeof(IAMStreamConfig).GUID, out config);
            IAMStreamConfig videoStreamConfig = config as IAMStreamConfig;
            if (videoStreamConfig == null)
            {
                throw new Exception("Failed to get IAMStreamConfig");
            }
            // Get the existing format block
            hr = videoStreamConfig.GetFormat(out mediaType);
            DsError.ThrowExceptionForHR(hr);
            // copy out the videoinfoheader
            VideoInfoHeader videoInfoHeader = new VideoInfoHeader();
            Marshal.PtrToStructure(mediaType.formatPtr, videoInfoHeader);
            // if overriding the framerate, set the frame rate
            if (iFrameRate > 0)
            {
                videoInfoHeader.AvgTimePerFrame = 10000000 / iFrameRate;
            }
            // if overriding the width, set the width
            if (iWidth > 0)
            {
                videoInfoHeader.BmiHeader.Width = iWidth;
            }
            // if overriding the Height, set the Height
            if (iHeight > 0)
            {
                videoInfoHeader.BmiHeader.Height = iHeight;
            }
            // Copy the media structure back
            Marshal.StructureToPtr(videoInfoHeader, mediaType.formatPtr, false);
            // Set the new format
            hr = videoStreamConfig.SetFormat(mediaType);
            DsError.ThrowExceptionForHR(hr);
            DsUtils.FreeAMMediaType(mediaType);
            mediaType = null;
        }

        private void SaveSizeInfo(ISampleGrabber sampleGrabber)
        {
            int hr;
            // Get the media type from the SampleGrabber
            AMMediaType media = new AMMediaType();
            hr = sampleGrabber.GetConnectedMediaType(media);
            DsError.ThrowExceptionForHR(hr);
            if ((media.formatType != FormatType.VideoInfo) || (media.formatPtr == IntPtr.Zero))
            {
                throw new NotSupportedException("Unknown Grabber Media Format");
            }
            // Grab the size info
            VideoInfoHeader videoInfoHeader = (VideoInfoHeader)Marshal.PtrToStructure(media.formatPtr, typeof(VideoInfoHeader));
            _previewStride = _previewWidth * (videoInfoHeader.BmiHeader.BitCount / 8);
            DsUtils.FreeAMMediaType(media);
            media = null;
        }
        public void SetupVideoWindow()
        {
            int hr = 0;
            // Set the video window to be a child of the PictureBox
            hr = this.videoWindow.put_Owner(_pictureBox.Handle);
            DsError.ThrowExceptionForHR(hr);
            hr = this.videoWindow.put_WindowStyle(WindowStyle.Child);

            DsError.ThrowExceptionForHR(hr);
            // Make the video window visible, now that it is properly positioned
            hr = this.videoWindow.put_Visible(OABool.True);
            DsError.ThrowExceptionForHR(hr);
            // Set the video position
            Rectangle rc = _pictureBox.ClientRectangle;
            //hr = videoWindow.SetWindowPosition(0, 0, _previewWidth, _previewHeight);
            hr = videoWindow.SetWindowPosition(0, 0, _pictureBox.Width, _pictureBox.Height);
            DsError.ThrowExceptionForHR(hr);
        }

        public int SampleCB(double SampleTime, IMediaSample pSample)
        {
            //throw new NotImplementedException();
            return 0;
        }

        public void Snap()
        {
            isGrab = true;
        }
        public int BufferCB(double SampleTime, nint pBuffer, int BufferLen)
        {
            if (isGrab)
            {
                Bitmap v = new Bitmap(_previewWidth, _previewHeight, _previewStride, PixelFormat.Format24bppRgb, pBuffer);
                //Bitmap v = new Bitmap(100, 100, _previewStride, PixelFormat.Format24bppRgb, pBuffer);
                //v.RotateFlip(RotateFlipType.Rotate180FlipX);
                v.RotateFlip(RotateFlipType.RotateNoneFlipY);

                isGrab = false;
                using MemoryStream ms = new MemoryStream();
                v.Save(ms, ImageFormat.Jpeg);
                SnapAction?.Invoke(ms.ToArray());
            }
            return 0;
        }

        public void ChangeCamera(string deviceName)
        {
            throw new NotImplementedException();
        }

        public void SizeChange()
        {
            SetupVideoWindow();
        }

        public void StopCamera()
        {
            if (videoWindow != null)
            {
                videoWindow.put_Visible(OABool.False);
                videoWindow.put_Owner(IntPtr.Zero);
            }
            // Remove filter graph from the running object table.
            if (rot != null)
            {
                rot.Dispose();
                rot = null;
            }
            // Release DirectShow interfaces.
            Marshal.ReleaseComObject(this.mediaControl);
            this.mediaControl = null;
            Marshal.ReleaseComObject(this.videoWindow);
            this.videoWindow = null;
            Marshal.ReleaseComObject(this.graphBuilder);
            this.graphBuilder = null;
            Marshal.ReleaseComObject(this.captureGraphBuilder);
            this.captureGraphBuilder = null;
        }

        public void Dispose()
        {
            this.Dispose();
        }

    }
}
