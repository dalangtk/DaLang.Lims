namespace DaLang.Lims.Pathology.Contracts.PathologySetting.Dto;

/// <summary>
/// 病理配置新增输入
/// </summary>
public class PathologySettingAddInput
{
    /// <summary>
    ///工作流
    ///</summary>
    public string? WFCode { get; set; }
    /// <summary>
    ///审核模式 1初复诊 2复诊
    ///</summary>
    public int AuditType { get; set; } = 2;
    /// <summary>
    ///前缀
    ///</summary>
    public string? SampleNoSymbol { get; set; }
    /// <summary>
    ///审核人Id
    ///</summary>
    public long? ReviewUserId { get; set; }
    /// <summary>
    ///审核人
    ///</summary>
    public string? ReviewUserName { get; set; }
    /// <summary>
    ///报告周期
    ///</summary>
    public int? ReportCycle { get; set; }
    /// <summary>
    /// 允许单医生
    /// </summary>
    public bool CanSameUserReport { get; set; } = false;
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
