namespace DaLang.Lims.BaseData.Contracts.AskRule.Dto;

/// <summary>
/// 问询规则明细更新输入
/// </summary>
public class BaseAskRuleDetailUpdateInput : BaseAskRuleDetailAddInput
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
}
