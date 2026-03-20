using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.Pretreatment.Contracts.BaseSorter.Dto;

/// <summary>
/// 分拣仪器新增输入
/// </summary>
public class BaseSorterAddInput
{
    /// <summary>分拣仪代码</summary>
    public string? SorterCode { get; set; }
    /// <summary>分拣仪名称</summary>
    public string? SorterName { get; set; }
    /// <summary>架子数</summary>
    public int? ShelfCount { get; set; }
    /// <summary>备注</summary>
    public string? Remark { get; set; }
    /// <summary>排序</summary>
    [Required(ErrorMessage = "排序不能为空")]
    public int Sort { get; set; }
    /// <summary>启用</summary>
    [Required(ErrorMessage = "启用不能为空")]
    public bool IsValid { get; set; } = true;
}
