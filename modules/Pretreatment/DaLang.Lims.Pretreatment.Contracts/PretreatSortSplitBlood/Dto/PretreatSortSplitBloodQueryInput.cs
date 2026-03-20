namespace DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood.Dto;

/// <summary>标本分血分页查询条件输入</summary>
public partial class PretreatSortSplitBloodQueryInput
{
    public string? Barcode { get; set; }
    public DateTime? BeginDate { get; set; }
    public DateTime? EndDate { get; set; }
}
