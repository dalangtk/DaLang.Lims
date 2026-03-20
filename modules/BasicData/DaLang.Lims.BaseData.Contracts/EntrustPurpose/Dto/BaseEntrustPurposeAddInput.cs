using System.ComponentModel.DataAnnotations;

namespace DaLang.Lims.BaseData.Contracts.EntrustPurpose.Dto;

/// <summary>
/// 委托目的新增输入
/// </summary>
public class BaseEntrustPurposeAddInput
{
    /// <summary>
    /// 目的代码
    /// </summary>
    [Required(ErrorMessage = "目的代码不能为空")]
    public string PurCode { get; set; }
    /// <summary>
    /// 问询规则
    /// </summary>
    public string? AskRuleCode { get; set; }
    /// <summary>
    /// 起始时间
    /// </summary>
    public DateTime? BeginTime { get; set; }
    /// <summary>
    /// 截止时间
    /// </summary>
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 接收周期
    /// </summary>
    public string? ReceiveDays { get; set; }
    /// <summary>
    /// 接收时间
    /// </summary>
    public string? ReceiveTime { get; set; }
    /// <summary>
    /// 客户代码
    /// </summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 标本类型
    /// </summary>
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 客户反向判定
    /// </summary>
    public bool? IsCustomerReverse { get; set; }
    /// <summary>
    /// 委托医院代码
    /// </summary>
    public string EntrustHospitalCode { get; set; }
    /// <summary>
    /// 委托医院名称
    /// </summary>
    public string EntrustHospitalName { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    [Required(ErrorMessage = "排序不能为空")]
    public int Sort { get; set; }
    /// <summary>启用</summary>
    [Required(ErrorMessage = "启用不能为空")]
    public bool IsValid { get; set; }
}
