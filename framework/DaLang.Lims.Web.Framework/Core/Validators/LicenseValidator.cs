using DaLang.Lims.Web.Common.Helpers;
using DaLang.Lims.Web.Framework.Core.Configs;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace DaLang.Lims.Web.Framework.Core.Validators;

public class LicenseValidator
{
    private readonly AppConfig _config;
    private bool _isValid = false;
    private DateTime? _expiry;

    public LicenseValidator(AppConfig config)
    {
        _config = config;
    }

    public bool Validate()
    {
        var licenseKey = _config.License.LicenseKey;
        var publicKey = _config.License.PublicKey;
        if (string.IsNullOrEmpty(licenseKey) || string.IsNullOrEmpty(publicKey))
            return false;

        try
        {
            var parts = licenseKey.Split('|');
            if (parts.Length != 2) return false;
            var data = Convert.FromBase64String(parts[0]);
            var sig = Convert.FromBase64String(parts[1]);
            using var rsa = RSA.Create();
            rsa.FromXmlString(publicKey);
            if (!rsa.VerifyData(data, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
                return false;
            var plain = Encoding.UTF8.GetString(data);
            var segments = plain.Split('|');
            if (segments.Length != 2) return false;
            var machineCode = segments[0];
            var expiry = DateTime.ParseExact(segments[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);
            return machineCode == MachineHelper.GetMachineCode() && expiry >= DateTime.UtcNow;
        }
        catch (Exception)
        {
            return false;
        }
    }
    public bool IsValid => _isValid;
    public DateTime? Expiry => _expiry;
}
