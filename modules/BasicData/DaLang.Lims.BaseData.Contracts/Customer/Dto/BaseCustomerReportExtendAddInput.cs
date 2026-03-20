namespace DaLang.Lims.BaseData.Contracts.Customer.Dto;

/// <summary>
/// 客户报告扩展新增输入
/// </summary>
public class BaseCustomerReportExtendAddInput
{
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    ///logo
    ///</summary>
    public string? CustomerLogo { get; set; }
    /// <summary>
    ///客户签章
    ///</summary>
    public string? CustomerSignature { get; set; }
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
    ///报告主标题
    ///</summary>
    public string? ReportMainTitle { get; set; }
    /// <summary>
    ///报告子标题
    ///</summary>
    public string? ReportSubTitle { get; set; }
    /// <summary>
    ///地址
    ///</summary>
    public string? Address { get; set; }
    /// <summary>
    ///电话
    ///</summary>
    public string? Telephone { get; set; }
    /// <summary>
    ///生成抬头类型
    ///</summary>
    public int? GenerateTitleType { get; set; }
    /// <summary>
    ///允许查看报告途径
    ///</summary>
    public string? CanSearchReportChannel { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    /// 纸张类型 0 A5 1 A4
    /// </summary>
    public int PaperType { get; set; } = 0;
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
