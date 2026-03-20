namespace DaLang.Lims.BaseData.Contracts.AskRule.Dto;

/// <summary>
/// 问询规则更新输入
/// </summary>
public class BaseAskRuleUpdateInput : BaseAskRuleAddInput
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
}
