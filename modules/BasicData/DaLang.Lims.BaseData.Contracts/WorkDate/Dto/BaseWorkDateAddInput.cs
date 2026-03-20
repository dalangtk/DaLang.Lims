using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.BaseData.Contracts.WorkDate.Dto;

/// <summary>
/// 工作日新增输入
/// </summary>
public class BaseWorkDateAddInput
{
    /// <summary>
    /// 日期
    /// </summary>
    public DateTime? WorkDate { get; set; }
    /// <summary>
    /// 日期类型
    /// </summary>
    public int? DateType { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; }
}
