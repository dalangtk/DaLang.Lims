namespace DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood.Dto;

/// <summary>
/// 标本分血输入
/// </summary>
public class SplitBloodInput
{
    public string Barcode { get; set; }
    public string? SampleTypeCode { get; set; }
}
