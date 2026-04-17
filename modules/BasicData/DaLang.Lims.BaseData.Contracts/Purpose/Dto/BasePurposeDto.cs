using DaLang.Lims.BaseData.Core.Enum;

namespace DaLang.Lims.BaseData.Contracts.Purpose.Dto;

/// <summary>
/// 检验目的查询结果输出
///</summary>
public partial class BasePurposeDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///组别名称
    ///</summary>
    public string GroupName { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string? PurCode { get; set; }
    /// <summary>
    ///目的名称
    ///</summary>
    public string PurName { get; set; }
    /// <summary>
    ///目的简写
    ///</summary>
    public string? PurNameAB { get; set; }
    /// <summary>
    ///目的英文
    ///</summary>
    public string? PurNameEN { get; set; }
    /// <summary>
    ///标本类型代码
    ///</summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    ///标本类型名称
    ///</summary>
    public string? SampleTypeName { get; set; }
    /// <summary>
    ///临床意义
    ///</summary>
    public string? ClinicalSence { get; set; }
    /// <summary>
    ///建议与解释
    ///</summary>
    public string? Suggestions { get; set; }
    /// <summary>
    /// 检验目的类型0 普检 1特检 2病理
    /// </summary>
    public PurposeTypeEnum PurposeType { get; set; } = PurposeTypeEnum.Routine;
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
