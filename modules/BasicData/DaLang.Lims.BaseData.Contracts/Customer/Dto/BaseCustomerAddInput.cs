namespace DaLang.Lims.BaseData.Contracts.Customer.Dto;

public class BaseCustomerAddInput
{
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    ///客户名称
    ///</summary>
    public string? CustomerName { get; set; }
    /// <summary>
    ///客户简称
    ///</summary>
    public string? CustomerNameAB { get; set; }
    /// <summary>
    ///区域
    ///</summary>
    public string? Area { get; set; }
    /// <summary>
    ///详细地址
    ///</summary>
    public string? Address { get; set; }
    /// <summary>
    ///经度
    ///</summary>
    public string? Longitude { get; set; }
    /// <summary>
    ///纬度
    ///</summary>
    public string? Latitude { get; set; }
    /// <summary>
    ///客户等级
    ///</summary>
    public string? CustomerLevel { get; set; }
    /// <summary>
    ///客户类型
    ///</summary>
    public string? CustomerType { get; set; }
    /// <summary>
    ///客户性质
    ///</summary>
    public string? CustomerNature { get; set; }
    /// <summary>
    ///合作模式
    ///</summary>
    public string? CollaborationMode { get; set; }
    /// <summary>
    ///联系人
    ///</summary>
    public string? Contacts { get; set; }
    /// <summary>
    ///联系电话
    ///</summary>
    public string? ContactPhone { get; set; }
    /// <summary>
    ///危急值联系人
    ///</summary>
    public string? CriticalContacts { get; set; }
    /// <summary>
    ///危急值联系电话
    ///</summary>
    public string? CriticalContactPhone { get; set; }
    /// <summary>
    ///信息录入方式
    ///</summary>
    public string? DoubleInputType { get; set; }
    /// <summary>
    ///合作方式
    ///</summary>
    public string? CooperateType { get; set; }
    /// <summary>
    ///首次合作时间
    ///</summary>
    public DateTime? FirstCooperateTime { get; set; }
    /// <summary>
    /// 客户分类
    /// </summary>
    public string? CustomerClassification { get; set; }
    /// <summary>
    /// 归属机构
    /// </summary>
    public long? BelongToTenant { get; set; }
    /// <summary>
    ///备注
    ///</summary>
    public string? Remark { get; set; }
    /// <summary>
    ///排序
    ///</summary>
    public int Sort { get; set; }
    /// <summary>
    ///启用
    ///</summary>
    public bool IsValid { get; set; }
}
