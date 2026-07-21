namespace DaLang.Lims.Client.Handler.Camera;

public interface ICameraDevice
{
    Action<byte[]> SnapAction { get; set; }
    string CurrentDeviceName { get; }
    List<string> GetDevices();
    void StartCamera(string deviceName);
    void ChangeCamera(string deviceName);
    void SizeChange();
    void Snap();
    void StopCamera();
}
