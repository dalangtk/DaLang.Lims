namespace DaLang.Lims.Pretreatment.Contracts.PretreatSortSplitBlood.Dto;

/// <summary>标本分血新增输入</summary>
public partial class PretreatSortSplitBloodAddInput
{
    /// <summary>
    ///条码
    ///</summary>
    public string Barcode { get; set; }
    /// <summary>
    ///标本类型代码
    ///</summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 标本类型名称
    /// </summary>
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
    /// 接收时间
    /// </summary>
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    ///分血管数
    ///</summary>
    public int? SplitTubeCnt { get; set; }
    /// <summary>
    ///分血状态
    ///</summary>
    public int? SplitBloodStatus { get; set; }
    /// <summary>
    ///分血人id
    ///</summary>
    public long? SplitBloodUserId { get; set; }
    /// <summary>
    ///分血人姓名
    ///</summary>
    public string? SplitBloodUserName { get; set; }
    /// <summary>
    ///分血时间
    ///</summary>
    public DateTime? SplitBloodTime { get; set; }

}
