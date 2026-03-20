using DaLang.Lims.Pretreatment.Core.Enum;
using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood.Dto;

public class SplitBloodOutput
{
    public SplitBloodStatusEnum Status { get; set; }
    public string Barcode { get; set; }
    public List<CodeNameDto> SampleTypeList { get; set; }
    public List<PretreatSortSplitBloodDto> SplitBlood { get; set; } 
    public List<PretreatSortSplitBloodDetailDto> SplitBloodDetail { get; set; }
}
