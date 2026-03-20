using DaLang.Lims.Shared.Contracts.ApplyPurpose.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.Sorting.Dto;

public class SortingPurpose : ApplyPurposeUpdateInput
{
    public string PatientName { get; set; }
    public string CustomerCode { get; set; }
    public string CustomerName { get; set; }
    public string PatientTypeCode { get; set; }
    public string GenderCode { get; set; }
    public string? SortRuleCode { get; set; }
    public string? SortRuleName { get; set; }
    public string? SequenceCode { get; set; }
    public string? SequenceName { get; set; }
    public long ApplyItemId { get; set; }
    public string ItemCode { get; set; }
    public string ItemName { get; set; }
    public string? ItemNamePersonalize { get; set; }
    public string SortingGroup { get; set; }
    public string SplitGroup { get; set; }
    public long SortInfoCode { get; set; }
    public long SortUserId { get; set; }
    public string SortUserName { get; set; }
    public string ShelfBarcode { get; set; }
    public string ShelfName { get; set; }
    public int HolePosition { get; set; }
    public string InstrumentItemCode { get; set; }
    public long? TaskId { get; set; }
}
