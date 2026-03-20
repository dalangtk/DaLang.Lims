namespace DaLang.Lims.Shared.Contracts.ExamTask.Dto;

/// <summary>
/// 检验任务更新输入
/// </summary>
public class ExamTaskUpdateInput
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
    ///组别名称
    ///</summary>
    public string GroupName { get; set; }
    /// <summary>
    ///客户代码
    ///</summary>
    public string? CustomerCode { get; set; }
    /// <summary>
    ///客户名称
    ///</summary>
    public string? CustomerName { get; set; }
    /// <summary>
    ///目的代码
    ///</summary>
    public string? PurCodes { get; set; }
    /// <summary>
    ///目的名称
    ///</summary>
    public string? PurNames { get; set; }
    /// <summary>
    /// 工作流
    /// </summary>
    public string? WFCode { get; set; }
    /// <summary>
    ///流水号
    ///</summary>
    public string? SampleNo { get; set; }
    /// <summary>
    ///标本类型
    ///</summary>
    public string SampleTypeCode { get; set; }
    /// <summary>
    ///标本类型名称
    ///</summary>
    public string SampleTypeName { get; set; }
    /// <summary>
    ///预计检测日期
    ///</summary>
    public DateTime? EstimatedTestDate { get; set; }
    /// <summary>
    ///预计报告日期
    ///</summary>
    public DateTime? EstimatedReportTime { get; set; }
    /// <summary>
    ///分拣人
    ///</summary>
    public long? SortUserId { get; set; }
    /// <summary>
    ///代码
    ///</summary>
    public string? SortUserName { get; set; }
    /// <summary>
    ///分拣时间
    ///</summary>
    public DateTime? SortTime { get; set; }
    /// <summary>
    ///分拣号
    ///</summary>
    public long? SortInfoCode { get; set; }
    /// <summary>
    ///架子条码
    ///</summary>
    public string? ShelfBarcode { get; set; }
    /// <summary>
    ///架子名称
    ///</summary>
    public string? ShelfName { get; set; }
    /// <summary>
    ///孔位
    ///</summary>
    public int? HolePosition { get; set; }
    /// <summary>
    /// 分拣规则代码
    /// </summary>
    public string? SortRuleCode { get; set; }
    /// <summary>
    /// 分拣规则名称
    /// </summary>
    public string? SortRuleName { get; set; }
    /// <summary>
    /// 序列代码
    /// </summary>
    public string? SequenceCode { get; set; }
    /// <summary>
    /// 序列名称
    /// </summary>
    public string? SequenceName { get; set; }
    /// <summary>
    ///分拣批次
    ///</summary>
    public long? SortBatchNo { get; set; }
    /// <summary>
    ///委托医院代码
    ///</summary>
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    ///委托医院名称
    ///</summary>
    public string? EntrustHospitalName { get; set; }
    /// <summary>
    ///委托状态
    ///</summary>
    public int? EntrustStatus { get; set; } = 0;
    /// <summary>
    ///委托确认人Id
    ///</summary>
    public long? EntrustConfirmId { get; set; }
    /// <summary>
    ///委托确认人
    ///</summary>
    public string? EntrustConfirmName { get; set; }
    /// <summary>
    ///委托确认时间
    ///</summary>
    public DateTime? EntrustConfirmTime { get; set; }
    /// <summary>
    ///委托人Id
    ///</summary>
    public long? EntrustId { get; set; }
    /// <summary>
    ///委托人
    ///</summary>
    public string? EntrustName { get; set; }
    /// <summary>
    ///委托时间
    ///</summary>
    public DateTime? EntrustTime { get; set; }
    /// <summary>
    ///委托批次
    ///</summary>
    public long? EntrustBatchNo { get; set; }
    /// <summary>
    ///预计问询时间
    ///</summary>
    public DateTime? EstimateAskTime { get; set; }
    /// <summary>
    ///接收时间
    ///</summary>
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    /// 交接状态
    /// </summary>
    public int HandoverStatus { get; set; } = 0;
    /// <summary>
    ///交接人Id
    ///</summary>
    public long? HandoverId { get; set; }
    /// <summary>
    ///交接人
    ///</summary>
    public string? HandoverName { get; set; }
    /// <summary>
    ///交接时间
    ///</summary>
    public DateTime? HandoverTime { get; set; }
    /// <summary>
    ///交接批次
    ///</summary>
    public long? HandoverBatchNo { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    ///申请目的Id
    ///</summary>
    public string? ApplyPurposeIds { get; set; }
}
