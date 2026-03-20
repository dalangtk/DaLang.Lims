using System.ComponentModel;
using DaLang.Lims.Web.Framework.Core.Attributes;

namespace DaLang.Lims.Web.Framework.Core.Consts;

/// <summary>
/// 缓存键
/// </summary>
[ScanCacheKeys]
public static partial class CacheKeys
{
    /// <summary>
    /// 用户权限缓存（按钮集合）
    /// </summary>
    public const string KeyUserButton = "sys_user_button:";

    /// <summary>
    /// 用户机构缓存
    /// </summary>
    public const string KeyUserOrg = "sys_user_org:";

    /// <summary>
    /// 角色最大数据范围缓存
    /// </summary>
    public const string KeyRoleMaxDataScope = "sys_role_maxDataScope:";

    /// <summary>
    /// 在线用户缓存
    /// </summary>
    public const string KeyUserOnline = "sys_user_online:";

    /// <summary>
    /// 图形验证码缓存
    /// </summary>
    public const string KeyVerCode = "sys_verCode:";

    /// <summary>
    /// 手机验证码缓存
    /// </summary>
    public const string KeyPhoneVerCode = "sys_phoneVerCode:";

    /// <summary>
    /// 密码错误次数缓存
    /// </summary>
    public const string KeyErrorPasswordCount = "sys_errorPasswordCount:";

    /// <summary>
    /// 租户缓存
    /// </summary>
    public const string KeyTenant = "sys_tenant";

    /// <summary>
    /// 常量下拉框
    /// </summary>
    public const string KeyConst = "sys_const:";

    /// <summary>
    /// 所有缓存关键字集合
    /// </summary>
    public const string KeyAll = "sys_keys";

    /// <summary>
    /// SqlSugar二级缓存
    /// </summary>
    public const string SqlSugar = "sys_sqlSugar:";

    /// <summary>
    /// 开放接口身份缓存
    /// </summary>
    public const string KeyOpenAccess = "sys_open_access:";

    /// <summary>
    /// 开放接口身份随机数缓存
    /// </summary>
    public const string KeyOpenAccessNonce = "sys_open_access_nonce:";

    /// <summary>
    /// 登录黑名单
    /// </summary>
    public const string KeyBlacklist = "sys_blacklist:";
    /// <summary>
    /// 验证码 admin:captcha:guid
    /// </summary>
    [Description("验证码")]
    public const string Captcha = "admin:captcha:";

    /// <summary>
    /// 密码加密 admin:password:encrypt:guid
    /// </summary>
    [Description("密码加密")]
    public const string PassWordEncrypt = "admin:password:encrypt:";

    /// <summary>
    /// 用户权限 admin:user:permissions:用户主键
    /// </summary>
    [Description("用户权限")]
    public const string UserPermission = "admin:user:permission:";

    /// <summary>
    /// 数据权限 admin:user:data:permission:用户主键
    /// </summary>
    [Description("数据权限")]
    public const string DataPermission = "admin:user:data:permission:";

    /// <summary>
    /// 短信验证码 admin:sms:code:guid
    /// </summary>
    [Description("短信验证码")]
    public const string SmsCode = "admin:sms:code:";

    /// <summary>
    /// 系统参数缓存 sys_param:
    /// </summary>
    [Description("系统参数缓存")]
    public const string SystemParam = "admin:sys_param:";

    /// <summary>
    /// 系统字典缓存 sys_dict:
    /// </summary>
    [Description("系统字典缓存")]
    public const string SystemDict = "sys_dict:";
    /// <summary>
    /// 获取短信验证码缓存键
    /// </summary>
    /// <param name="mobile">手机号</param>
    /// <param name="code">唯一码</param>
    /// <returns></returns>
    public static string GetSmsCodeKey(string mobile, string code) => $"{SmsCode}{mobile}:{code}";

    /// <summary>
    /// 获取数据权限缓存键
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <param name="apiPath">请求接口路径</param>
    /// <returns></returns>
    public static string GetDataPermissionKey(long userId, string apiPath = null)
    {
        if (apiPath.IsNull())
        {
            apiPath = AppInfo.CurrentDataPermissionApiPath;
        }

        return $"{DataPermission}{userId}{(apiPath.NotNull() ? (":" + apiPath) : "")}";
    }
    /// <summary>
    /// 获取数据权限缓存键
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    public static string GetUserPermissionKey(long userId) => $"{UserPermission}{userId}";

    /// <summary>
    /// 获取数据权限模板
    /// </summary>
    /// <param name="userId">用户Id</param>
    /// <returns></returns>
    public static string GetDataPermissionPattern(long userId) => $"{DataPermission}{userId}*";
}