namespace DaLang.Lims.Pretreatment.Contracts.DataImport.Dto;

public class QueryImportDataInput
{
    public bool OnlyMySelf { get; set; }
    public DateTime Begin { get; set; }
    public DateTime End { get; set; }
    public string? CustomerCode { get; set; }
    public string? Barcode { get; set; }
    public string? PatientName { get; set; }
}
