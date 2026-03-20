namespace DaLang.Lims.ReportTemplate.Contracts.ReportTemplate.Dto;

public class ReportExtendDto
{
    /// <summary>
    /// 客户代码
    /// </summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 机构logo
    /// </summary>
    public string? TenantLogo { get; set; }
    /// <summary>
    /// 机构签章
    /// </summary>
    public string? TenantSignature { get; set; }
    /// <summary>
    /// 客户logo
    /// </summary>
    public string? CustomerLogo { get; set; }
    /// <summary>
    /// 客户签章
    /// </summary>
    public string? CustomerSignature { get; set; }
    /// <summary>
    /// 机构主标题
    /// </summary>
    public string? TenantMainTitle { get; set; }
    /// <summary>
    /// 客户主标题
    /// </summary>
    public string? CustomerMainTitle { get; set; }
    /// <summary>
    /// 机构子标题
    /// </summary>
    public string? TenantSubTitle { get; set; }
    /// <summary>
    /// 客户子标题
    /// </summary>
    public string? CustomerSubTitle { get; set; }
    /// <summary>
    /// 机构地址
    /// </summary>
    public string? TenantAddress { get; set; }
    /// <summary>
    /// 客户地址
    /// </summary>
    public string? CustomerAddress { get; set; }
    /// <summary>
    /// 机构电话
    /// </summary>
    public string? TenantTelephone { get; set; }
    /// <summary>
    /// 客户电话
    /// </summary>
    public string? CustomerTelephone { get; set; }
    /// <summary>
    ///是否生成报告
    ///</summary>
    public bool? IsGenerateReport { get; set; }
    /// <summary>
    ///报告优先级
    ///</summary>
    public int? ReportPriority { get; set; }
    /// <summary>
    ///图片DPI
    ///</summary>
    public int? ImageDpi { get; set; }
    /// <summary>
    ///报告语言
    ///</summary>
    public string? LanguageType { get; set; }
    /// <summary>
    ///生成抬头类型
    ///</summary>
    public int? GenerateTitleType { get; set; }
    /// <summary>
    ///允许查看报告途径
    ///</summary>
    public string? CanSearchReportChannel { get; set; }
    /// <summary>
    /// 纸张类型 0 A5 1 A4
    /// </summary>
    public int PaperType { get; set; } = 0;

    /// <summary>
    ///报告主标题
    ///</summary>
    public string? ReportMainTitle => GenerateTitleType == 0 ? TenantMainTitle : CustomerMainTitle;
    /// <summary>
    ///报告子标题
    ///</summary>
    public string? ReportSubTitle => GenerateTitleType == 0 ? TenantSubTitle : CustomerSubTitle;
    /// <summary>
    ///签章
    ///</summary>
    public string? ReportSignature => GenerateTitleType == 0 ? TenantSignature : CustomerSignature;
    /// <summary>
    ///logo
    ///</summary>
    public string? ReportLogo => GenerateTitleType == 0 ? TenantLogo : CustomerLogo;
    /// <summary>
    ///地址
    ///</summary>
    public string? Address => GenerateTitleType == 0 ? TenantAddress : CustomerAddress;
    /// <summary>
    ///电话
    ///</summary>
    public string? Telephone => GenerateTitleType == 0 ? TenantTelephone : CustomerTelephone;
}
