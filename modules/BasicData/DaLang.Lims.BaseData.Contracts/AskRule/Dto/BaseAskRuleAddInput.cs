namespace DaLang.Lims.BaseData.Contracts.AskRule.Dto;

/// <summary>
/// 问询规则新增输入
/// </summary>
public class BaseAskRuleAddInput
{
    /// <summary>
    ///问询规则代码
    ///</summary>
    public string AskRuleCode { get; set; }
    /// <summary>
    ///问询规则名称
    ///</summary>
    public string AskRuleName { get; set; }
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
    public bool IsValid { get; set; } = true;
}
