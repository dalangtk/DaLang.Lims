using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace DaLang.Lims.Web.Common.Helpers;

public class MachineHelper
{
    public static string GetMachineCode()
    {
        // 获取所有网卡的 MAC 地址（过滤掉虚拟网卡）
        var macs = NetworkInterface.GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up)
            .Select(nic => nic.GetPhysicalAddress().ToString())
            .Where(mac => !string.IsNullOrEmpty(mac) && mac.Length >= 12)
            .OrderBy(mac => mac)
            .FirstOrDefault();

        var raw = $"{macs}";
        using var sha = SHA256.Create();
        var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(raw));
        return Convert.ToBase64String(bytes).TrimEnd('=');
    }
}
