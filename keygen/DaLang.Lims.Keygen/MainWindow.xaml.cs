using DaLang.Lims.Web.Common.Helpers;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DaLang.Lims.Keygen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string privateKey = "<RSAKeyValue>\r\n     <Modulus>pLXu9yXqemXD/ZOpNI5GuTgkYaGfhlkJknViNbha0yT2ziLt31b4/jsNf4SsSjsk9bKNTuiUz2uEztSfNwqEwtXWkiMiUfQd+1UEsW/dioTHBgWLTQWORYr2lBNdbAEzDJSyVUkbNrk1dGqumoKGhKrIXQ6XZp2dk2VeHPd4ulcuI1Mnmq6TkGVUu+cJ89vkpbZHpzo8vnSaj2DDTkmKBhdxpbgX1KyXi5fdJAdZRwj2rguCENEI4U6AGo/s9n54UlmNtZ+PzwCEOoIly8e6woCEDIeaXACYHOrIMsZn6PKzsuHTKRN5w621I5rnV1rTLmge3Hb607nfyygSqyNHow==</Modulus>\r\n     <Exponent>AQAB</Exponent>\r\n     <D>lHpE7ppJYwi1wrMHhw7yhikr8q7FpVBCJrqWS6LijQ/3RB+MmxZF4rfNFTS+MzRZvvq0E3vqGYjs8Z4N4Iok35XwlVACsioDlPsw/dq/tQy4dJ8u0cG+XMZGPXmvb5stL5wjnmoGlDAlFcrNlw8uwA1MDUcICELxN1vbmPUk6aQSsuv8qlzabJZdqIucZpOZzeMxw2d+nlGe51ZjA+R88AiNaElHwHtgiSVrazuy2f3UHpwE6Eukb6Jz5ci8ymXykp0RSb49jeVJ7c5EPZ6xq2uKgZ1fneCbjadWfPZ9WJ04gPkK7ZKbzytbKh3YhseIvbZHHICoSFWGThUOBip0EQ==</D>\r\n     <P>7zcNCCgIMEIWUZE+EUp0UG9uLGdiEGJU6Mc+lk+Eup+ZS0IIPX64PMhq24olb++o4zVcztJdz1UCts2GXlYEjxa+O9g3cAAqU2cXLOxeXAh+ZQZP8PbxfmkNZXT9M5cFcfFoYOPSq9Yt13TPjpPT2cRvuXW8tmAq6W2Uqkw+l6U=</P>\r\n     <Q>sESVKcbdUBYEKJ6NQHIT3hQzXD7ggqcxK2UNaMaK6g5omUlNl4JkN6kovWkB1qqUWhoADgY18wyS593eiLqM8EJKR5mnbW7ruBaFlADyaqa2HcEEEIv6s7AgK1FWcyW+ElB/8CGt25ZM+YoLQDM/N7iCrONPRqPFZNkPGrRt/6c=</Q>\r\n     <DP>j5gQLpE5OR6ynwv8ogx7fb0l+WmlqAgeyAyyOTBKkGw1bMOahq+GkJYN7vMiPeszChCCQXRt84MoOetteTtRSUNxtiK4RHcU8TO06baJfd1rYeFEI3VYyXTuIbT0OP+yuQ8chOy9DYFnyynHXeLHDLWqCz3tpdxcZfrtfrx95jE=</DP>\r\n     <DQ>cd3b/gDZNsMCLLs/xEHtU6j5BL4Zik03FjFRWD3pvwlPyLXxyc4Rr2M3TiRna6UF8K6bGnQopbmZrO9GskzQfuRT160K20asP19NKy7xarQHMXFejtHeQfWk0lbY4cmf33ThMN6Q0Slm0Ey8t9xjGju3LrgonhltipgtibLE3EE=</DQ>\r\n     <InverseQ>2gIRPEOl5EnddLIfEfQbmqTySMbD4fcEwTLT77pf2LWOwBgK927FXvc+dkBcEdi6+mWOy13mU1Uw0fpM4NA38LxoVhG1ioYNFUyWFtEEe1+ZQ7yKBW7fm+Mdig3x32Ko3F+lkgTJfG9hv7jAZbVHFF63eDokiRwm6zfc5h+Aln0=</InverseQ>\r\n   </RSAKeyValue>";

        const string publicKey = "<RSAKeyValue>\r\n     <Modulus>pLXu9yXqemXD/ZOpNI5GuTgkYaGfhlkJknViNbha0yT2ziLt31b4/jsNf4SsSjsk9bKNTuiUz2uEztSfNwqEwtXWkiMiUfQd+1UEsW/dioTHBgWLTQWORYr2lBNdbAEzDJSyVUkbNrk1dGqumoKGhKrIXQ6XZp2dk2VeHPd4ulcuI1Mnmq6TkGVUu+cJ89vkpbZHpzo8vnSaj2DDTkmKBhdxpbgX1KyXi5fdJAdZRwj2rguCENEI4U6AGo/s9n54UlmNtZ+PzwCEOoIly8e6woCEDIeaXACYHOrIMsZn6PKzsuHTKRN5w621I5rnV1rTLmge3Hb607nfyygSqyNHow==</Modulus>\r\n     <Exponent>AQAB</Exponent>\r\n   </RSAKeyValue>";

        bool _isValid;
        DateTime _expiry;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void GenButton_Click(object sender, RoutedEventArgs e)
        {
            var machineCode1 = MachineHelper.GetMachineCode();
            var machineCode = txtIdentifier.Text?.Trim();
            if (string.IsNullOrWhiteSpace(machineCode))
            {
                MessageBox.Show("请输入机器码", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var key = GenerateLicense(machineCode, DateTime.UtcNow.AddYears(1), privateKey);

            txtRegistrationCode.Text = key;

            string GenerateLicense(string machineCode, DateTime expiry, string privateKeyPem)
            {
                var plain = $"{machineCode}|{expiry:yyyy-MM-dd}";
                var data = Encoding.UTF8.GetBytes(plain);
                using var rsa = RSA.Create();
                rsa.FromXmlString(privateKeyPem);
                var sig = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                return Convert.ToBase64String(data) + "|" + Convert.ToBase64String(sig);
            }
        }

        private void ResoveButton_Click(object sender, RoutedEventArgs e)
        {
            var key = txtKey.Text?.Trim();
            if (string.IsNullOrWhiteSpace(key))
            {
                MessageBox.Show("请输入机器码", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var ret = Validate(key);
            if (ret.Item1)
            {
                txtResove.Text = $"机器码: {ret.Item2};过期时间: {ret.Item3?.ToString("yyyy-MM-dd")}";
            }
            else
            {
                MessageBox.Show("注册码无效或已过期", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        public (bool, string, DateTime?) Validate(string licenseKey)
        {
            if (string.IsNullOrEmpty(licenseKey))
                return (false, null, null);

            try
            {
                var parts = licenseKey.Split('|');
                if (parts.Length != 2) return (false, null, null);
                var data = Convert.FromBase64String(parts[0]);
                var sig = Convert.FromBase64String(parts[1]);
                using var rsa = RSA.Create();
                rsa.FromXmlString(publicKey);
                if (!rsa.VerifyData(data, sig, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1))
                    return (false, null, null);
                var plain = Encoding.UTF8.GetString(data);
                var segments = plain.Split('|');
                if (segments.Length != 2) return (false, null, null);
                var machineCode = segments[0];
                var expiry = DateTime.ParseExact(segments[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                return (machineCode == MachineHelper.GetMachineCode() && expiry >= DateTime.UtcNow, machineCode, expiry);
            }
            catch (Exception ex)
            {
                return (false, null, null);
            }
        }

        (string machineCode, DateTime expiry, string signature)? DecodeLicense(string license)
        {
            try
            {
                var decoded = Encoding.UTF8.GetString(Convert.FromBase64String(license));
                var segments = decoded.Split('|');
                if (segments.Length != 3) return null;
                var code = segments[0];
                var expiry = DateTime.ParseExact(segments[1], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var sig = segments[2];
                return (code, expiry, sig);
            }
            catch { return null; }
        }
    }

    public class LicensePayload
    {
        public string MachineCode { get; set; }
        public DateTime Expiry { get; set; } // 反序列化时自动识别 ISO 8601
    }
}