using DaLang.Lims.Web.Common.Enums;

namespace DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;

#pragma warning disable CS1591,CS8618

public class StartSortingInput
{
    public string SorterCode { get; set; }
    public SortModeEnum SortMode { get; set; } = SortModeEnum.UnUseShelf;
    public List<StartSortingDetailInput> SortDetailList { get; set; } = new List<StartSortingDetailInput>();
}
public class StartSortingDetailInput
{
    public string ShelfBarcode { get; set; }
    public int ShelfPosition { get; set; }
}

#pragma warning restore CS1591,CS8618