using DaLang.Lims.Pretreatment.Contracts.BaseSorterShelf.Dto;
using DaLang.Lims.Pretreatment.Contracts.BaseSorterShelfRule.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.BaseSorter.Dto;

public class SorterAndDetailAddInput
{
    public BaseSorterAddInput SorterInfo { get; set; }
    public List<BaseSorterShelfAddInput> SorterShelfList { get; set; }
    public List<BaseSorterShelfRuleAddInput> SorterShelfRuleList { get; set; }
}
public class SorterAndDetailUpdateInput
{
    public BaseSorterUpdateInput SorterInfo { get; set; }
    public List<BaseSorterShelfUpdateInput> SorterShelfList { get; set; }
    public List<BaseSorterShelfRuleUpdateInput> SorterShelfRuleList { get; set; }
}
