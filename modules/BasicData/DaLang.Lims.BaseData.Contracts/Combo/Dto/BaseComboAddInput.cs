using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.BaseData.Contracts.Combo.Dto;

public class BaseComboAddInput
{
    /// <summary>
    /// 套餐代码
    /// </summary>
    public string? ComboCode { get; set; }
    /// <summary>
    /// 套餐名称
    /// </summary>
    [Required(ErrorMessage = "套餐名称不能为空")]
    public required string ComboName { get; set; }
    /// <summary>
    /// 客户
    /// </summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 起始时间
    /// </summary>
    public DateTime? BeginDate { get; set; }
    /// <summary>
    /// 截止时间
    /// </summary>
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// 标本类型
    /// </summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    [Required(ErrorMessage = "排序不能为空")]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    [Required(ErrorMessage = "启用不能为空")]
    public bool IsValid { get; set; } = true;
}
