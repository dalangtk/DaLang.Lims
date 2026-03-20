using System;

namespace DaLang.Lims.Web.Framework.Services.Parameter.Dto;

/// <summary>
/// 系统参数分页查询条件输入
/// </summary>
public class ParameterQueryInput
{
    public string ParamName { get; set; } = string.Empty;
}

public class ParamGetOrSetInput : ParameterQueryInput
{
    public string? DefaultValue { get; set; } = "";
    public TimeSpan? ExpireTime { get; set; } = TimeSpan.FromHours(12);
}
