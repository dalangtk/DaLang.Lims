using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Dto;
using DaLang.Lims.Web.Common.Helpers;

namespace DaLang.Lims.Web.Framework.Core.Helpers;

/// <summary>
/// 用户帮助类
/// </summary>
[InjectSingleton]
public class UserHelper
{
    /// <summary>
    /// 检查密码
    /// </summary>
    /// <param name="password"></param>
    public void CheckPassword(string password)
    {
        if (!PasswordHelper.Verify(password))
        {
            throw ResultOutput.Exception("密码为字母+数字+可选特殊字符，长度在6-16之间");
        }
    }
}