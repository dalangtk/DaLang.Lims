using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Shared.Domain.ApplyPurpose;

/// <summary>
/// 申请单目的 实体类
/// </summary>
/// <remarks>申请目的</remarks>
[SugarTable(TableName = "apply_purpose")]
public partial class ApplyPurposeEntity : EntityTenant
{
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
    /// 条码
    /// </summary>
    /// <remarks>条码</remarks>
    [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 套餐代码
    /// </summary>
    /// <remarks>套餐代码</remarks>
    [SugarColumn(ColumnName = "ComboCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ComboCode { get; set; }
    /// <summary>
    /// 套餐名称
    /// </summary>
    /// <remarks>套餐名称</remarks>
    [SugarColumn(ColumnName = "ComboName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ComboName { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string PurCode { get; set; }
    /// <summary>
    /// 目的名称
    /// </summary>
    /// <remarks>目的名称</remarks>
    [SugarColumn(ColumnName = "PurName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string PurName { get; set; }
    /// <summary>
    /// 个性化目的名称
    /// </summary>
    /// <remarks>个性化目的名称</remarks>
    [SugarColumn(ColumnName = "PurNamePersonalize", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? PurNamePersonalize { get; set; }
    /// <summary>
    /// 目的数量
    /// </summary>
    /// <remarks>目的数量</remarks>
    [SugarColumn(ColumnName = "PurAmount", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "1", DecimalDigits = 11)]
    public int PurAmount { get; set; } = 1;
    /// <summary>
    /// 上机项目代码
    /// </summary>
    /// <remarks>上机项目代码</remarks>
    [SugarColumn(ColumnName = "InstrumentItemCodes", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? InstrumentItemCodes { get; set; }
    /// <summary>
    /// 工作流
    /// </summary>
    /// <remarks>工作流</remarks>
    [SugarColumn(ColumnName = "WorkFlowType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? WorkFlowType { get; set; }
    /// <summary>
    /// 检测计划代码
    /// </summary>
    /// <remarks>检测计划代码</remarks>
    [SugarColumn(ColumnName = "TestExamPlanCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? TestExamPlanCode { get; set; }
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
    [SugarColumn(ColumnName = "EstimateTestDate", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "date")]
    public DateTime? EstimateTestDate { get; set; }
    /// <summary>
    /// 计划报告日期
    /// </summary>
    /// <remarks>计划报告日期</remarks>
    [SugarColumn(ColumnName = "EstimateReportTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EstimateReportTime { get; set; }
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
    /// 预计问询时间
    /// </summary>
    /// <remarks>预计问询时间</remarks>
    [SugarColumn(ColumnName = "EstimateAskTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EstimateAskTime { get; set; }
    /// <summary>
    /// 原始组别代码
    /// </summary>
    /// <remarks>原始组别代码</remarks>
    [SugarColumn(ColumnName = "OriginalGroupCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string OriginalGroupCode { get; set; }
    /// <summary>
    /// 原始组别名称
    /// </summary>
    /// <remarks>原始组别名称</remarks>
    [SugarColumn(ColumnName = "OriginalGroupName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string OriginalGroupName { get; set; }
    /// <summary>
    /// 增项
    /// </summary>
    /// <remarks>增项</remarks>
    [SugarColumn(ColumnName = "AddType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "0", DecimalDigits = 11)]
    public int? AddType { get; set; } = 0;
    /// <summary>
    /// 样本状态
    /// </summary>
    /// <remarks>样本状态</remarks>
    [SugarColumn(ColumnName = "SampleStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int? SampleStatus { get; set; }
    /// <summary>
    /// 样本状态名称
    /// </summary>
    /// <remarks>样本状态名称</remarks>
    [SugarColumn(ColumnName = "SampleStatusName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SampleStatusName { get; set; }
    /// <summary>
    /// 接收人Id
    /// </summary>
    /// <remarks>接收人Id</remarks>
    [SugarColumn(ColumnName = "ReceiveId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ReceiveId { get; set; }
    /// <summary>
    /// 接收人姓名
    /// </summary>
    /// <remarks>接收人姓名</remarks>
    [SugarColumn(ColumnName = "ReceiveName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ReceiveName { get; set; }
    /// <summary>
    /// 接收时间
    /// </summary>
    /// <remarks>接收时间</remarks>
    [SugarColumn(ColumnName = "ReceiveTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    /// 标本数量
    /// </summary>
    /// <remarks>标本数量</remarks>
    [SugarColumn(ColumnName = "SampleCnt", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "1", DecimalDigits = 2)]
    public int? SampleCnt { get; set; } = 1;
    /// <summary>
    /// 检测班次
    /// </summary>
    /// <remarks>检测班次</remarks>
    [SugarColumn(ColumnName = "TestShift", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? TestShift { get; set; }
    /// <summary>
    /// 检测系列
    /// </summary>
    /// <remarks>检测系列</remarks>
    [SugarColumn(ColumnName = "TestSeries", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? TestSeries { get; set; }
    /// <summary>
    /// 分拣状态
    /// </summary>
    /// <remarks>分拣状态</remarks>
    [SugarColumn(ColumnName = "SortStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "0", DecimalDigits = 11)]
    public int? SortStatus { get; set; } = 0;
    /// <summary>
    /// 数据来源
    /// </summary>
    /// <remarks>数据来源</remarks>
    [SugarColumn(ColumnName = "DataSource", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int? DataSource { get; set; }
    /// <summary>
    /// 批次号
    /// </summary>
    /// <remarks>批次号</remarks>
    [SugarColumn(ColumnName = "BatchNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? BatchNo { get; set; }
    /// <summary>
    /// 批次扫码顺序
    /// </summary>
    /// <remarks>批次扫码顺序</remarks>
    [SugarColumn(ColumnName = "BatchScanOrder", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 8)]
    public int? BatchScanOrder { get; set; }
    /// <summary>
    /// 检验任务Id
    /// </summary>
    /// <remarks>检验任务Id</remarks>
    [SugarColumn(ColumnName = "TaskId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? TaskId { get; set; }
    /// <summary>
    /// 是否计费
    /// </summary>
    /// <remarks>是否计费</remarks>
    [SugarColumn(ColumnName = "IsCharging", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "1")]
    public bool IsCharging { get; set; } = true;
    /// <summary>
    /// 财务生成标记
    /// </summary>
    /// <remarks>财务生成标记</remarks>
    [SugarColumn(ColumnName = "FinanceStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2, DefaultValue = "0")]
    public int FinanceStatus { get; set; } = 0;
}

#pragma warning restore CS8618

