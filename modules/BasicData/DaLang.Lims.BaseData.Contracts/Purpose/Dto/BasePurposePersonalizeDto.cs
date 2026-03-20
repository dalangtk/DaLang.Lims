namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

///<summary>
///目的定制查询结果输出
///</summary>
public partial class BasePurposePersonalizeDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string PurCode { get; set; }
    /// <summary>
    ///标本类型
    ///</summary>
    public List<string> SampleTypeCode { get; set; }
    /// <summary>
    ///个性化目的名称
    ///</summary>
    public string? PurNamePersonalize { get; set; }
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
    /// <summary>
    /// 标本类型名称
    /// </summary>
    public string SampleTypeName { get; set; }
}
