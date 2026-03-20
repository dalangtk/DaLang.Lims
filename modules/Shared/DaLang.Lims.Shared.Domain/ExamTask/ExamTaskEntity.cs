using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Shared.Domain.ExamTask;

/// <summary>
/// 检验任务 实体类
/// </summary>
/// <remarks>检验任务</remarks>
[SugarTable(TableName = "exam_task")]
public partial class ExamTaskEntity : EntityTenant
{
    /// <summary>
    /// 条码
    /// </summary>
    /// <remarks>条码</remarks>
    [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string GroupCode { get; set; }
    /// <summary>
    /// 组别名称
    /// </summary>
    /// <remarks>组别名称</remarks>
    [SugarColumn(ColumnName = "GroupName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string GroupName { get; set; }
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 客户名称
    /// </summary>
    /// <remarks>客户名称</remarks>
    [SugarColumn(ColumnName = "CustomerName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? CustomerName { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCodes", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? PurCodes { get; set; }
    /// <summary>
    /// 目的名称
    /// </summary>
    /// <remarks>目的名称</remarks>
    [SugarColumn(ColumnName = "PurNames", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? PurNames { get; set; }
    /// <summary>
    /// 工作流
    /// </summary>
    /// <remarks>工作流</remarks>
    [SugarColumn(ColumnName = "WFCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? WFCode { get; set; }
    /// <summary>
    /// 流水号
    /// </summary>
    /// <remarks>流水号</remarks>
    [SugarColumn(ColumnName = "SampleNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? SampleNo { get; set; }
    /// <summary>
    /// 标本类型
    /// </summary>
    /// <remarks>标本类型</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string SampleTypeCode { get; set; }
    /// <summary>
    /// 标本类型名称
    /// </summary>
    /// <remarks>标本类型名称</remarks>
    [SugarColumn(ColumnName = "SampleTypeName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string SampleTypeName { get; set; }
    /// <summary>
    /// 预计检测日期
    /// </summary>
    /// <remarks>预计检测日期</remarks>
    [SugarColumn(ColumnName = "EstimatedTestDate", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "date")]
    public DateTime? EstimatedTestDate { get; set; }
    /// <summary>
    /// 预计报告日期
    /// </summary>
    /// <remarks>预计报告日期</remarks>
    [SugarColumn(ColumnName = "EstimatedReportTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EstimatedReportTime { get; set; }
    /// <summary>
    /// 分拣人
    /// </summary>
    /// <remarks>分拣人</remarks>
    [SugarColumn(ColumnName = "SortUserId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? SortUserId { get; set; }
    /// <summary>
    /// 代码
    /// </summary>
    /// <remarks>代码</remarks>
    [SugarColumn(ColumnName = "SortUserName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? SortUserName { get; set; }
    /// <summary>
    /// 分拣时间
    /// </summary>
    /// <remarks>分拣时间</remarks>
    [SugarColumn(ColumnName = "SortTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? SortTime { get; set; }
    /// <summary>
    /// 分拣号
    /// </summary>
    /// <remarks>分拣号</remarks>
    [SugarColumn(ColumnName = "SortInfoCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? SortInfoCode { get; set; }
    /// <summary>
    /// 架子条码
    /// </summary>
    /// <remarks>架子条码</remarks>
    [SugarColumn(ColumnName = "ShelfBarcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? ShelfBarcode { get; set; }
    /// <summary>
    /// 架子名称
    /// </summary>
    /// <remarks>架子名称</remarks>
    [SugarColumn(ColumnName = "ShelfName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ShelfName { get; set; }
    /// <summary>
    /// 孔位
    /// </summary>
    /// <remarks>孔位</remarks>
    [SugarColumn(ColumnName = "HolePosition", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? HolePosition { get; set; }
    /// <summary>
    /// 分拣批次
    /// </summary>
    /// <remarks>分拣批次</remarks>
    [SugarColumn(ColumnName = "SortBatchNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? SortBatchNo { get; set; }
    /// <summary>
    /// 规则代码
    /// </summary>
    /// <remarks>规则代码</remarks>
    [SugarColumn(ColumnName = "SortRuleCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? SortRuleCode { get; set; }
    /// <summary>
    /// 规则名称
    /// </summary>
    /// <remarks>规则名称</remarks>
    [SugarColumn(ColumnName = "SortRuleName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SortRuleName { get; set; }
    /// <summary>
    /// 序列代码
    /// </summary>
    /// <remarks>序列代码</remarks>
    [SugarColumn(ColumnName = "SequenceCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SequenceCode { get; set; }
    /// <summary>
    /// 序列名称
    /// </summary>
    /// <remarks>序列名称</remarks>
    [SugarColumn(ColumnName = "SequenceName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? SequenceName { get; set; }
    /// <summary>
    /// 委托医院代码
    /// </summary>
    /// <remarks>委托医院代码</remarks>
    [SugarColumn(ColumnName = "EntrustHospitalCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    /// 委托医院名称
    /// </summary>
    /// <remarks>委托医院名称</remarks>
    [SugarColumn(ColumnName = "EntrustHospitalName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? EntrustHospitalName { get; set; }
    /// <summary>
    /// 委托状态
    /// </summary>
    /// <remarks>委托状态</remarks>
    [SugarColumn(ColumnName = "EntrustStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "0", DecimalDigits = 1)]
    public int? EntrustStatus { get; set; } = 0;
    /// <summary>
    /// 委托确认人Id
    /// </summary>
    /// <remarks>委托确认人Id</remarks>
    [SugarColumn(ColumnName = "EntrustConfirmId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? EntrustConfirmId { get; set; }
    /// <summary>
    /// 委托确认人
    /// </summary>
    /// <remarks>委托确认人</remarks>
    [SugarColumn(ColumnName = "EntrustConfirmName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? EntrustConfirmName { get; set; }
    /// <summary>
    /// 委托确认时间
    /// </summary>
    /// <remarks>委托确认时间</remarks>
    [SugarColumn(ColumnName = "EntrustConfirmTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EntrustConfirmTime { get; set; }
    /// <summary>
    /// 委托人Id
    /// </summary>
    /// <remarks>委托人Id</remarks>
    [SugarColumn(ColumnName = "EntrustId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? EntrustId { get; set; }
    /// <summary>
    /// 委托人
    /// </summary>
    /// <remarks>委托人</remarks>
    [SugarColumn(ColumnName = "EntrustName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? EntrustName { get; set; }
    /// <summary>
    /// 委托时间
    /// </summary>
    /// <remarks>委托时间</remarks>
    [SugarColumn(ColumnName = "EntrustTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EntrustTime { get; set; }
    /// <summary>
    /// 委托批次
    /// </summary>
    /// <remarks>委托批次</remarks>
    [SugarColumn(ColumnName = "EntrustBatchNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? EntrustBatchNo { get; set; }
    /// <summary>
    /// 预计问询时间
    /// </summary>
    /// <remarks>预计问询时间</remarks>
    [SugarColumn(ColumnName = "EstimateAskTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EstimateAskTime { get; set; }
    /// <summary>
    /// 接收时间
    /// </summary>
    /// <remarks>接收时间</remarks>
    [SugarColumn(ColumnName = "ReceiveTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    /// 交接状态
    /// </summary>
    [SugarColumn(ColumnName = "HandoverStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11,DefaultValue = "0")]
    public int HandoverStatus { get; set; } = 0;
    /// <summary>
    /// 交接人Id
    /// </summary>
    /// <remarks>交接人Id</remarks>
    [SugarColumn(ColumnName = "HandoverId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? HandoverId { get; set; }
    /// <summary>
    /// 交接人
    /// </summary>
    /// <remarks>交接人</remarks>
    [SugarColumn(ColumnName = "HandoverName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? HandoverName { get; set; }
    /// <summary>
    /// 交接时间
    /// </summary>
    /// <remarks>交接时间</remarks>
    [SugarColumn(ColumnName = "HandoverTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? HandoverTime { get; set; }
    /// <summary>
    /// 交接批次
    /// </summary>
    /// <remarks>交接批次</remarks>
    [SugarColumn(ColumnName = "HandoverBatchNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? HandoverBatchNo { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }
    /// <summary>
    /// 申请目的Id
    /// </summary>
    /// <remarks>申请目的Id</remarks>
    [SugarColumn(ColumnName = "ApplyPurposeIds", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 512)]
    public string? ApplyPurposeIds { get; set; }
}

#pragma warning restore CS8618

