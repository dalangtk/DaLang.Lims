#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Contracts.ItemReference.Dto;

/// <summary>
/// 参考范围
/// </summary>
public class BaseItemReferenceDto
{
    /// <summary>
    /// Id
    /// </summary>
    public long Id { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string ItemCode { get; set; }
    /// <summary>
    ///结果类型，定量、定性等
    ///</summary>
    public string? ResultType { get; set; }
    /// <summary>
    ///方法学
    ///</summary>
    public string? Method { get; set; }
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    ///仪器
    ///</summary>
    public string? InstrumentCode { get; set; }
    /// <summary>
    ///试剂
    ///</summary>
    public string? ReagentCode { get; set; }
    /// <summary>
    ///标本类型
    ///</summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    ///性别
    ///</summary>
    public string? GenderCode { get; set; }
    /// <summary>
    ///委托医院
    ///</summary>
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    ///年龄下限
    ///</summary>
    public int? AgeLowLimit { get; set; }
    /// <summary>
    ///年龄上限
    ///</summary>
    public int? AgeUpperLimit { get; set; }
    /// <summary>
    ///年龄单位
    ///</summary>
    public string? AgeUnit { get; set; }
    /// <summary>
    ///警告范围
    ///</summary>
    public string? WarningRange { get; set; }
    /// <summary>
    ///参考范围
    ///</summary>
    public string? ReferenceRange { get; set; }
    /// <summary>
    ///危急值范围
    ///</summary>
    public string? CriticalRange { get; set; }
    /// <summary>
    /// 报告显示范围
    /// </summary>
    public string? DisplayRange { get; set; }
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

#pragma warning restore CS8618