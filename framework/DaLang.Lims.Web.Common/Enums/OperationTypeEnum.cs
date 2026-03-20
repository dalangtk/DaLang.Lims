using System.ComponentModel;

namespace DaLang.Lims.Web.Common.Enums;

/// <summary>
/// 操作类型枚举
/// </summary>
public enum OperationTypeEnum
{
    [Description("接收")]
    Receive = 0,
    [Description("分拣")]
    Sorting = 1,
    [Description("交接")]
    Handover = 2,
    [Description("检验")]
    Testing = 3,
    [Description("初审")]
    FirstCheck = 4,
    [Description("复审")]
    SecondCheck = 5,
    [Description("取消审核")]
    UnChecked = 6,
    [Description("分血")]
    SplitBlood = 7,
    [Description("打印后取消审核")]
    PrintedUnChecked = 8,
    [Description("增项")]
    AddItem = 9,
    [Description("退项")]
    DeleteItem = 10,
    [Description("生成报告")]
    CreateReport = 11,
    [Description("移除报告")]
    DeleteReport = 12,
    [Description("取消检测")]
    CancelTest = 13,
    [Description("打印报告")]
    PrintReport = 14,
}

/// <summary>
/// 批量或者单个枚举
/// </summary>
public enum ExecuteTypeEnum
{
    /// <summary>
    /// 单个
    /// </summary>
    [Description("单个")]
    Single = 0,
    /// <summary>
    /// 批量
    /// </summary>
    [Description("批量")]
    Batch = 1,
}