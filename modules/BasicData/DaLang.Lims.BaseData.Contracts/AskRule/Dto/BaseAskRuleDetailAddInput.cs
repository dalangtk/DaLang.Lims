namespace DaLang.Lims.BaseData.Contracts.AskRule.Dto;

/// <summary>
/// 问询规则明细新增输入
/// </summary>
public class BaseAskRuleDetailAddInput
{
    /// <summary>
    ///问询规则代码
    ///</summary>
    public string AskRuleCode { get; set; }
    /// <summary>
    ///委托周期
    ///</summary>
    public string? EntrustCycle { get; set; }
    /// <summary>
    ///问询日期
    ///</summary>
    public int? AskDay { get; set; }
    /// <summary>
    ///问询时间
    ///</summary>
    public string? AskTime { get; set; }
    /// <summary>
    ///包含节假日
    ///</summary>
    public bool IsIncludeHoliday { get; set; } = false;
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; } = true;
}
