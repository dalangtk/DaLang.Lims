namespace DaLang.Lims.Shared.Contracts.ExamTaskDetail.Dto;

/// <summary>
/// 检验任务明细查询输出
/// </summary>
public class ExamTaskDetailDto
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
    /// <summary>
    ///条码
    ///</summary>
    public string Barcode { get; set; }
    /// <summary>
    ///组别代码
    ///</summary>
    public string GroupCode { get; set; }
    /// <summary>
    ///任务Id
    ///</summary>
    public long? TaskId { get; set; }
    /// <summary>
    ///申请项目Id
    ///</summary>
    public long ApplyItemId { get; set; }
    /// <summary>
    ///套餐代码
    ///</summary>
    public string? ComboCode { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    public string? PurCode { get; set; }
    /// <summary>
    ///上机项目代码
    ///</summary>
    public string? InstrumentItemCode { get; set; }
    /// <summary>
    ///项目代码
    ///</summary>
    public string ItemCode { get; set; }
    /// <summary>
    ///项目名称
    ///</summary>
    public string ItemName { get; set; }
    /// <summary>
    ///个性化项目名称
    ///</summary>
    public string? ItemNamePersonalize { get; set; }
    /// <summary>
    ///接入检验
    ///</summary>
    public int InTest { get; set; } = 0;

}
