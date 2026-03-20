namespace DaLang.Lims.BaseData.Contracts.SampleType.Dto;

///<summary>
///标本类型查询结果输出
///</summary>
public partial class BaseSampleTypeDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///标本类型代码
    ///</summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    ///标本类型名称
    ///</summary>
    public string? SampleTypeName { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    public string? Gender { get; set; }
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
}
