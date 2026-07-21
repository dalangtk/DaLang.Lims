using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;

namespace DaLang.Lims.Client.Handler.Util;

public static class CommonHelper
{
    public static void WriteObjectToFile(object data, string filePath)
    {
        JArray array = JArray.FromObject(data);
        if (array.Count == 0) return;

        var firstObj = array[0] as JObject;
        if (firstObj == null)
            throw new InvalidOperationException("Data is not array of objects.");

        var propertyNames = firstObj.Properties().Select(p => p.Name).ToList();

        using (var writer = new StreamWriter(filePath))
        {
            writer.WriteLine(string.Join("\t", propertyNames));

            foreach (JObject obj in array)
            {
                var values = propertyNames.Select(name => obj[name]?.ToString() ?? "");
                writer.WriteLine(string.Join("\t", values));
            }
        }
    }
    public static bool checkHasInstalledSoftWare(string displayName)
    {
        var localMachineRegistry = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
            Environment.Is64BitOperatingSystem
                ? RegistryView.Registry64
                : RegistryView.Registry32);

        var uninstallNode = localMachineRegistry.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall");
        foreach (string subKeyName in uninstallNode.GetSubKeyNames())
        {
            Microsoft.Win32.RegistryKey subKey = uninstallNode.OpenSubKey(subKeyName);
            object disName = subKey.GetValue("DisplayName");
            if (disName != null)
            {
                if (disName.ToString() == displayName)
                {
                    return true;
                    // MessageBox.Show(displayName.ToString()); 

                }
            }
        }
        return false;
    }

    public static async Task<string> UploadFile(MemoryStream ms,string fileName, string apiUrl, long examInfoId, bool isGross,string accessToken)
    {
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromMinutes(5); // 大文件上传超时设置
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        using var formData = new MultipartFormDataContent();

        // 添加文件流
        //await using var fileStream = File.OpenRead(filePath);
        var fileContent = new StreamContent(ms);// new StreamContent(fileStream);
        fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
        //fileContent.Headers.Add("Authorization", $"Bearer {accessToken}");
        formData.Add(fileContent, "file", fileName);

        // 添加其他参数
        formData.Add(new StringContent(examInfoId.ToString()), "examInfoId");
        formData.Add(new StringContent(isGross.ToString().ToLower()), "isGrossExamination");

        var response = await httpClient.PostAsync(apiUrl, formData);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (response.IsSuccessStatusCode)
        {
            return responseBody;
        }
        else
        {
            throw new Exception($"上传失败!{response.StatusCode}): {responseBody}");
        }
    }
}
