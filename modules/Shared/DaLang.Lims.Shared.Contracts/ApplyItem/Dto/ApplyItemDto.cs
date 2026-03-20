namespace DaLang.Lims.Shared.Contracts.ApplyItem.Dto;

///<summary>
///申请单项目查询结果输出
///</summary>
public partial class ApplyItemDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///目的Id
    ///</summary>
    public long ApplyPurposeId { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///组别名称
    ///</summary>
    public string GroupName { get; set; }
    /// <summary>
    ///条码
    ///</summary>
    public string Barcode { get; set; }
    /// <summary>
    ///套餐代码
    ///</summary>
    public string? ComboCode { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string PurCode { get; set; }
    /// <summary>
    ///上机项目代码
    ///</summary>
    public string? InstrumentItemCode { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string ItemCode { get; set; }
    /// <summary>
    ///项目名称
    ///</summary>
    public string ItemName { get; set; }
    /// <summary>
    ///个性化项目名称
    ///</summary>
    public string? ItemNamePersonalize { get; set; }

}
