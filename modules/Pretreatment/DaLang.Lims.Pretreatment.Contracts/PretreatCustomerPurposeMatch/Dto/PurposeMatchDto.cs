namespace DaLang.Lims.Pretreatment.Contracts.PretreatCustomerPurposeMatch.Dto;

///<summary>
///目的对照查询结果输出
///</summary>
public partial class PurposeMatchDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    ///客户目的代码
    ///</summary>
    public string? CustomerPurCode { get; set; }
    /// <summary>
    ///客户目的名称
    ///</summary>
    public string? CustomerPurName { get; set; }
    /// <summary>
    ///中心目的代码
    ///</summary>
    public string? CentralPurCode { get; set; }
    /// <summary>
    ///中心目的名称
    ///</summary>
    public string? CentralPurName { get; set; }
    /// <summary>
    /// 是否套餐
    /// </summary>
    public int IsCombo { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
}
