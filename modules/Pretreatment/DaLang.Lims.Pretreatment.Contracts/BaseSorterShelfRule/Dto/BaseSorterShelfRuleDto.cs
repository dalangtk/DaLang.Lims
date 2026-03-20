namespace DaLang.Lims.Pretreatment.Contracts.BaseSorterShelfRule.Dto;

/// <summary>
/// 架子规则查询结果输出
/// </summary>
public class BaseSorterShelfRuleDto : BaseSorterShelfRuleUpdateInput
{
    public string SortRuleName { get; set; }
    public string SequenceCode { get; set; }
    public string SequenceName { get; set; }
}
