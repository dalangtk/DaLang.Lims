namespace DaLang.Lims.Web.Framework.Services.Parameter.Dto;

/// <summary>
/// 系统参数新增输入
/// </summary>
public class ParameterAddInput
{
    /// <summary>
    ///参数名称
    ///</summary>
    public string? ParamName { get; set; }
    /// <summary>
    ///参数值
    ///</summary>
    public string? ParamValue { get; set; }
    /// <summary>
    ///备注
    ///</summary>
    public string? Remark { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
}
