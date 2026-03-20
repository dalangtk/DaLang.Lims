using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Exam.Domain.ExamCriticalValue;

/// <summary>
/// 危急值 实体类
/// </summary>
/// <remarks>危急值</remarks>
[SugarTable(TableName = "exam_critical_value")]
public partial class ExamCriticalValueEntity : EntityTenant
{
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? GroupCode { get; set; }
    /// <summary>
    /// 组别名称
    /// </summary>
    /// <remarks>组别名称</remarks>
    [SugarColumn(ColumnName = "GroupName", ColumnDataType = "varchar", Length = 32)]
    public string? GroupName { get; set; }
    /// <summary>
    /// 检验Id
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamInfoId", ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 结果Id
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamResultId", ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ExamResultId { get; set; }
    /// <summary>
    /// 条码
    /// </summary>
    /// <remarks>条码</remarks>
    [SugarColumn(ColumnName = "Barcode", ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 样本号
    /// </summary>
    /// <remarks>样本号</remarks>
    [SugarColumn(ColumnName = "SampleNo", ColumnDataType = "varchar", Length = 32)]
    public string SampleNo { get; set; }
    /// <summary>
    /// 检测日期
    /// </summary>
    /// <remarks>检测日期</remarks>
    [SugarColumn(ColumnName = "TestDate", ColumnDataType = "date")]
    public DateTime? TestDate { get; set; }
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCode", ColumnDataType = "varchar", Length = 16)]
    public string PurCode { get; set; }
    /// <summary>
    /// 目的名称
    /// </summary>
    /// <remarks>目的名称</remarks>
    [SugarColumn(ColumnName = "PurName", ColumnDataType = "varchar", Length = 64)]
    public string? PurName { get; set; }
    /// <summary>
    /// 上机项目代码
    /// </summary>
    /// <remarks>上机项目代码</remarks>
    [SugarColumn(ColumnName = "InstrumentItemCode", ColumnDataType = "varchar", Length = 16)]
    public string? InstrumentItemCode { get; set; }
    /// <summary>
    /// 项目代码
    /// </summary>
    /// <remarks>项目代码</remarks>
    [SugarColumn(ColumnName = "ItemCode", ColumnDataType = "varchar", Length = 16)]
    public string ItemCode { get; set; }
    /// <summary>
    /// 项目名称
    /// </summary>
    /// <remarks>项目名称</remarks>
    [SugarColumn(ColumnName = "ItemName", ColumnDataType = "varchar", Length = 32)]
    public string ItemName { get; set; }
    /// <summary>
    /// 检验结果
    /// </summary>
    /// <remarks>检验结果</remarks>
    [SugarColumn(ColumnName = "ItemResult", ColumnDataType = "varchar", Length = 128)]
    public string? ItemResult { get; set; }
    /// <summary>
    /// 危急值内容
    /// </summary>
    /// <remarks>危急值内容</remarks>
    [SugarColumn(ColumnName = "CriticalContent", ColumnDataType = "varchar", Length = 128)]
    public string? CriticalContent { get; set; }
    /// <summary>
    /// 复查时间
    /// </summary>
    /// <remarks>复查时间</remarks>
    [SugarColumn(ColumnName = "ReviewTime", ColumnDataType = "datetime")]
    public DateTime? ReviewTime { get; set; }
    /// <summary>
    /// 复查结果
    /// </summary>
    /// <remarks>复查结果</remarks>
    [SugarColumn(ColumnName = "ReviewResult", ColumnDataType = "varchar", Length = 128)]
    public string? ReviewResult { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    /// <remarks>联系人</remarks>
    [SugarColumn(ColumnName = "ContactName", ColumnDataType = "varchar", Length = 32)]
    public string? ContactName { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    /// <remarks>联系电话</remarks>
    [SugarColumn(ColumnName = "ContactPhone", ColumnDataType = "varchar", Length = 32)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 联系时间
    /// </summary>
    /// <remarks>联系时间</remarks>
    [SugarColumn(ColumnName = "ContactTime", ColumnDataType = "datetime")]
    public DateTime? ContactTime { get; set; }
    /// <summary>
    /// 复述内容
    /// </summary>
    /// <remarks>复述内容</remarks>
    [SugarColumn(ColumnName = "RepeatContent", ColumnDataType = "varchar", Length = 255)]
    public string? RepeatContent { get; set; }
    /// <summary>
    /// 处理情况及反馈
    /// </summary>
    /// <remarks>处理情况及反馈</remarks>
    [SugarColumn(ColumnName = "ProcessRemark", ColumnDataType = "varchar", Length = 255)]
    public string? ProcessRemark { get; set; }
    /// <summary>
    /// 处理人Id
    /// </summary>
    /// <remarks>处理人Id</remarks>
    [SugarColumn(ColumnName = "ProcessId", ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ProcessId { get; set; }
    /// <summary>
    /// 处理人
    /// </summary>
    /// <remarks>处理人</remarks>
    [SugarColumn(ColumnName = "ProcessName", ColumnDataType = "varchar", Length = 64)]
    public string? ProcessName { get; set; }
    /// <summary>
    /// 处理时间
    /// </summary>
    /// <remarks>处理时间</remarks>
    [SugarColumn(ColumnName = "ProcessTime", ColumnDataType = "datetime")]
    public DateTime? ProcessTime { get; set; }
    /// <summary>
    /// 是否取消
    /// </summary>
    /// <remarks>是否取消</remarks>
    [SugarColumn(ColumnName = "IsCancel", ColumnDataType = "tinyint", DefaultValue = "0")]
    public bool IsCancel { get; set; } = false;
    /// <summary>
    /// 取消原因
    /// </summary>
    /// <remarks>取消原因</remarks>
    [SugarColumn(ColumnName = "CancelReason", ColumnDataType = "varchar", Length = 128)]
    public string? CancelReason { get; set; }
    /// <summary>
    /// 处理状态
    /// </summary>
    /// <remarks>处理状态</remarks>
    [SugarColumn(ColumnName = "ProcessStatus", ColumnDataType = "int", DecimalDigits = 2, DefaultValue = "0")]
    public int ProcessStatus { get; set; } = 0;
}

#pragma warning restore CS8618

