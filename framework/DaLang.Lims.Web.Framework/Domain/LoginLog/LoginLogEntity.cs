using SqlSugar;

namespace DaLang.Lims.Web.Framework.Domain.LoginLog;

/// <summary>
/// 登录日志
/// </summary>
[SugarTable(TableName = "sys_login_log")]
public partial class LoginLogEntity : LogAbstract
{
}