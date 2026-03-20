using System.ComponentModel;

namespace DaLang.Lims.Web.Common.Enums;

/// <summary>
/// 样本状态枚举
/// </summary>
public enum SampleStatusEnum
{
    /// <summary>
    /// 初始
    /// </summary>
    [Description("初始")]
    Init = 1001,
    /// <summary>
    /// 确认
    /// </summary>
    [Description("确认")]
    Confirmed = 1011,
    /// <summary>
    /// 待分血
    /// </summary>
    [Description("待分血")]
    WaitSplitBlood = 1021,
    /// <summary>
    /// 已分血
    /// </summary>
    [Description("已分血")]
    SplitBlood = 1031,
    /// <summary>
    /// 分拣完成
    /// </summary>
    [Description("已分拣")]
    Sorted = 1041,
    /// <summary>
    /// 已交接
    /// </summary>
    [Description("已交接")]
    Handovered = 1051,
    /// <summary>
    /// 检验中
    /// </summary>
    [Description("待检验")]
    WaitTesting = 1060,
    /// <summary>
    /// 检验中
    /// </summary>
    [Description("检验中")]
    Testing = 1061,
    /// <summary>
    /// 初审
    /// </summary>
    [Description("初审")]
    FirstCheck = 1071,
    /// <summary>
    /// 复审
    /// </summary>
    [Description("复审")]
    SecondCheck = 1081,
    /// <summary>
    /// 已报告
    /// </summary>
    [Description("已报告")]
    Reported = 1091,
    /// <summary>
    /// 已打印
    /// </summary>
    [Description("已打印")]
    Printed = 1101,
    /// <summary>
    /// 取消报告
    /// </summary>
    [Description("取消报告")]
    ReportCancel = 1111,
    /// <summary>
    /// 报告延迟
    /// </summary>
    [Description("报告延迟")]
    ReportDelay = 1121,

}
