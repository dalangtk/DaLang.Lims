namespace DaLang.Lims.BaseData.Contracts.EntrustHospital.Dto;

/// <summary>
///委托医院 查询结果输出
///</summary>
public partial class BaseEntrustHospitalDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///委托医院代码
    ///</summary>
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    ///委托医院名称
    ///</summary>
    public string? EntrustHospitalName { get; set; }
    /// <summary>
    ///联系人
    ///</summary>
    public string? Contacts { get; set; }
    /// <summary>
    ///联系电话
    ///</summary>
    public string? ContactPhone { get; set; }
    /// <summary>
    ///拼音
    ///</summary>
    public string? PinYin { get; set; }
    /// <summary>
    ///五笔
    ///</summary>
    public string? WuBi { get; set; }
    /// <summary>
    ///自定义码
    ///</summary>
    public string? CustomCode { get; set; }
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
    /// <summary>
    ///创建者Id
    ///</summary>
    public long? ProId { get; set; }
    /// <summary>
    ///创建者姓名
    ///</summary>
    public string? ProName { get; set; }
    /// <summary>
    ///创建时间
    ///</summary>
    public DateTime ProTime { get; set; }
    /// <summary>
    ///修改者Id
    ///</summary>
    public long? ModId { get; set; }
    /// <summary>
    ///修改者姓名
    ///</summary>
    public string? ModName { get; set; }
    /// <summary>
    ///更新时间
    ///</summary>
    public DateTime? ModTime { get; set; }
    /// <summary>
    ///已修改
    ///</summary>
    public bool IsModified { get; set; }
}
