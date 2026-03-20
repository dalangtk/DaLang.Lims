using SqlSugar;
using System;
using DaLang.Lims.Web.Framework.Core.Entities;

namespace DaLang.Lims.Web.Framework.Domain.AppLog;

/// <summary>
/// 应用程序日志
/// </summary>
[SugarTable("sys_app_log")]
public partial class AppLogEntity : EntityTenant
{
    public DateTime Logged { get; set; }
    public string Level { get; set; }
    public string Message { get; set; }
    public string Logger { get; set; }
    public string Properties { get; set; }
    public string Callsite { get; set; }
    public string Exception { get; set; }
}
