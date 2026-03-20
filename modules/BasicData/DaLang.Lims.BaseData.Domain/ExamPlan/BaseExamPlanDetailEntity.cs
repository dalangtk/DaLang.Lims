using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.ExamPlan;

/// <summary>
/// 检测计划明细 实体类
/// </summary>
/// <remarks>检测计划明细</remarks>
[SugarTable(TableName = "base_exam_plan_detail")]
public partial class BaseExamPlanDetailEntity : EntityTenant
{
    /// <summary>
    /// 检测计划代码
    /// </summary>
    /// <remarks>检测计划代码</remarks>
    [SugarColumn(ColumnName = "ExamPlanCode", ColumnDataType = "varchar", Length = 8)]
    public string ExamPlanCode { get; set; }
    /// <summary>
    /// 接收时间
    /// </summary>
    /// <remarks>接收时间</remarks>
    [SugarColumn(ColumnName = "ReceiveTime", ColumnDataType = "datetime")]
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    /// 检测间隔
    /// </summary>
    /// <remarks>检测间隔</remarks>
    [SugarColumn(ColumnName = "TestInterval", ColumnDataType = "int", DecimalDigits = 4)]
    public int? TestInterval { get; set; }
    /// <summary>
    /// 报告间隔
    /// </summary>
    /// <remarks>报告间隔</remarks>
    [SugarColumn(ColumnName = "ReportInterval", ColumnDataType = "int", DecimalDigits = 4)]
    public int? ReportInterval { get; set; }
    /// <summary>
    /// 报告时间
    /// </summary>
    /// <remarks>报告时间</remarks>
    [SugarColumn(ColumnName = "ReportTimePoint", ColumnDataType = "varchar", Length = 16)]
    public string? ReportTimePoint { get; set; }
    /// <summary>
    /// 时间类型，点/秒/分/时/天等
    /// </summary>
    /// <remarks>时间类型，点/秒/分/时/天等</remarks>
    [SugarColumn(ColumnName = "TimePointType", ColumnDataType = "varchar", Length = 8)]
    public string? TimePointType { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }
}

#pragma warning restore CS8618

