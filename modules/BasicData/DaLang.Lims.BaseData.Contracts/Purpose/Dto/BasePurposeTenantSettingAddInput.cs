namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

/// <summary>
/// 目的机构设置新增输入
/// </summary>
public partial class BasePurposeTenantSettingAddInput
{
    /// <summary>
    ///目的代码
    ///</summary>
    public string PurCode { get; set; }
    /// <summary>
    ///是否启用
    ///</summary>
    public bool? IsEnable { get; set; }
    /// <summary>
    ///录入隐藏
    ///</summary>
    public bool? IsInputHide { get; set; }
    /// <summary>
    ///工作流
    ///</summary>
    public string? WorkFlowType { get; set; }
    /// <summary>
    ///检测计划
    ///</summary>
    public string? ExamPlan { get; set; }
    /// <summary>
    ///检测班次
    ///</summary>
    public string? TestShift { get; set; }
    /// <summary>
    ///检测系列
    ///</summary>
    public string? TestSeries { get; set; }
    /// <summary>
    ///检查性别
    ///</summary>
    public string? TestSex { get; set; }
    /// <summary>
    ///存储条件
    ///</summary>
    public string? StorageCondition { get; set; }
    /// <summary>
    ///存储周期
    ///</summary>
    public string? StorageCycle { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
}
