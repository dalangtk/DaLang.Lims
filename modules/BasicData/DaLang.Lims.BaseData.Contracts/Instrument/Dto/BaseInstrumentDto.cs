namespace DaLang.Lims.BaseData.Contracts.Instrument.Dto;

/// <summary>仪器
///查询结果输出
///</summary>
public partial class BaseInstrumentDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///仪器代码
    ///</summary>
    public string? InstrumentCode { get; set; }
    /// <summary>
    ///仪器名称
    ///</summary>
    public string? InstrumentName { get; set; }
    /// <summary>
    ///PinYin
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
    ///
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
