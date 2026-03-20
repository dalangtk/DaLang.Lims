namespace DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood.Dto;

public class PretreatSortSplitBloodDetailUpdateInput
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    /// 分血表Id
    /// </summary>
    public long SplitBloodId { get; set; }
    /// <summary>
    ///条码
    ///</summary>
    public string Barcode { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///组别名称
    ///</summary>
    public string GroupName { get; set; }
    /// <summary>
    ///标本类型代码
    ///</summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    ///标本类型名称
    ///</summary>
    public string? SampleTypeName { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string? PurCodes { get; set; }
    /// <summary>
    ///目的名称
    ///</summary>
    public string? PurNames { get; set; }
    /// <summary>
    /// 委托医院
    /// </summary>
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    /// 委托医院名称
    /// </summary>
    public string? EntrustHospitalName { get; set; }
}
