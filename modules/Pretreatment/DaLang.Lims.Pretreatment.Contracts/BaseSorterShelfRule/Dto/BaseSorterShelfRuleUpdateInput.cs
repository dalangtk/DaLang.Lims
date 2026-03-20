using DaLang.Lims.Web.Framework.Core.Enums;

namespace DaLang.Lims.Pretreatment.Contracts.BaseSorterShelfRule.Dto;

/// <summary>
/// 架子规则更新数据输入
/// </summary>
public class BaseSorterShelfRuleUpdateInput : BaseSorterShelfRuleAddInput
{
    public long Id { get; set; }
    public RowStatusEnum RowStatus { get; set; } = RowStatusEnum.UnChange;
}
