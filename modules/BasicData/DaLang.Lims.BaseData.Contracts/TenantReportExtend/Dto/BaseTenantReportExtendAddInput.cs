namespace DaLang.Lims.BaseData.Contracts.TenantReportExtend.Dto;

/// <summary>
/// 机构报告设置新增输入
/// </summary>
public class BaseTenantReportExtendAddInput
{
    /// <summary>
    ///logo
    ///</summary>
    public string? ReportLogo { get; set; }
    /// <summary>
    ///报告主标题
    ///</summary>
    public string? ReportMainTitle { get; set; }
    /// <summary>
    ///报告子标题
    ///</summary>
    public string? ReportSubTitle { get; set; }
    /// <summary>
    ///签章
    ///</summary>
    public string? ReportSignature { get; set; }
    /// <summary>
    ///地址
    ///</summary>
    public string? Address { get; set; }
    /// <summary>
    ///电话
    ///</summary>
    public string? Telephone { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
