using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8618
namespace DaLang.Lims.Shared.Domain.ExamResult;

/// <summary>
/// 检验结果 实体类
/// </summary>
/// <remarks>检验结果</remarks>
[SugarTable(TableName = "exam_result")]
public partial class ExamResultEntity : EntityTenant
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "TaskDetailId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long TaskDetailId { get; set; }
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamInfoId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long ExamInfoId { get; set; }
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
    [SugarColumn(ColumnName = "GroupName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? GroupName { get; set; }
    /// <summary>
    /// 条码
    /// </summary>
    /// <remarks>条码</remarks>
    [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 样本号
    /// </summary>
    /// <remarks>样本号</remarks>
    [SugarColumn(ColumnName = "SampleNo", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string SampleNo { get; set; }
    /// <summary>
    /// 检测日期
    /// </summary>
    /// <remarks>检测日期</remarks>
    [SugarColumn(ColumnName = "TestDate", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "date")]
    public DateTime? TestDate { get; set; }
    /// <summary>
    /// 套餐代码
    /// </summary>
    /// <remarks>套餐代码</remarks>
    [SugarColumn(ColumnName = "ComboCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ComboCode { get; set; }
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
    /// 上机项目代码
    /// </summary>
    /// <remarks>上机项目代码</remarks>
    [SugarColumn(ColumnName = "InstrumentItemCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? InstrumentItemCode { get; set; }
    /// <summary>
    /// 项目代码
    /// </summary>
    /// <remarks>项目代码</remarks>
    [SugarColumn(ColumnName = "ItemCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string ItemCode { get; set; }
    /// <summary>
    /// 项目名称
    /// </summary>
    /// <remarks>项目名称</remarks>
    [SugarColumn(ColumnName = "ItemName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string ItemName { get; set; }
    /// <summary>
    /// 个性化项目名称
    /// </summary>
    /// <remarks>个性化项目名称</remarks>
    [SugarColumn(ColumnName = "ItemNamePersonalize", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ItemNamePersonalize { get; set; }
    /// <summary>
    /// 英文名称
    /// </summary>
    /// <remarks>英文名称</remarks>
    [SugarColumn(ColumnName = "ItemNameEN", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? ItemNameEN { get; set; }
    /// <summary>
    /// 项目简称
    /// </summary>
    /// <remarks>项目简称</remarks>
    [SugarColumn(ColumnName = "ItemNameAB", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ItemNameAB { get; set; }
    /// <summary>
    /// 项目单位
    /// </summary>
    /// <remarks>项目单位</remarks>
    [SugarColumn(ColumnName = "ItemUnit", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ItemUnit { get; set; }
    /// <summary>
    /// 检验结果
    /// </summary>
    /// <remarks>检验结果</remarks>
    [SugarColumn(ColumnName = "ItemResult", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? ItemResult { get; set; }
    /// <summary>
    /// 高低标记
    /// </summary>
    /// <remarks>高低标记</remarks>
    [SugarColumn(ColumnName = "HLFlag", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 4)]
    public string? HLFlag { get; set; }
    /// <summary>
    /// 参考范围
    /// </summary>
    /// <remarks>参考范围</remarks>
    [SugarColumn(ColumnName = "ItemReference", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? ItemReference { get; set; }
    /// <summary>
    /// 警告值范围
    /// </summary>
    /// <remarks>警告值范围</remarks>
    [SugarColumn(ColumnName = "WarningRange", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? WarningRange { get; set; }
    /// <summary>
    /// 危急值范围
    /// </summary>
    /// <remarks>危急值范围</remarks>
    [SugarColumn(ColumnName = "CriticalRange", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? CriticalRange { get; set; }
    /// <summary>
    /// 报告显示范围
    /// </summary>
    /// <remarks>报告显示范围</remarks>
    [SugarColumn(ColumnName = "DisplayRange", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? DisplayRange { get; set; }
    /// <summary>
    /// 试剂代码
    /// </summary>
    /// <remarks>试剂代码</remarks>
    [SugarColumn(ColumnName = "ReagentCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ReagentCode { get; set; }
    /// <summary>
    /// 试剂名称
    /// </summary>
    /// <remarks>试剂名称</remarks>
    [SugarColumn(ColumnName = "ReagentName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ReagentName { get; set; }
    /// <summary>
    /// 仪器代码
    /// </summary>
    /// <remarks>仪器代码</remarks>
    [SugarColumn(ColumnName = "InstrumentCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? InstrumentCode { get; set; }
    /// <summary>
    /// 仪器名称
    /// </summary>
    /// <remarks>仪器名称</remarks>
    [SugarColumn(ColumnName = "InstrumentName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? InstrumentName { get; set; }
    /// <summary>
    /// 方法学代码
    /// </summary>
    /// <remarks>方法学代码</remarks>
    [SugarColumn(ColumnName = "MethodCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? MethodCode { get; set; }
    /// <summary>
    /// 方法学
    /// </summary>
    /// <remarks>方法学</remarks>
    [SugarColumn(ColumnName = "MethodName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? MethodName { get; set; }
    /// <summary>
    /// 结果来源1输入 2回传
    /// </summary>
    /// <remarks>结果来源1输入 2回传</remarks>
    [SugarColumn(ColumnName = "ResultSource", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? ResultSource { get; set; }
    /// <summary>
    /// 原始结果
    /// </summary>
    /// <remarks>原始结果</remarks>
    [SugarColumn(ColumnName = "OriginalValue", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 255)]
    public string? OriginalValue { get; set; }
    /// <summary>
    /// 报告显示
    /// </summary>
    /// <remarks>报告显示</remarks>
    [SugarColumn(ColumnName = "IsReportShow", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool? IsReportShow { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "ReportOrder", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ReportOrder { get; set; }
    /// <summary>
    /// 方法依据
    /// </summary>
    /// <remarks>方法依据</remarks>
    [SugarColumn(ColumnName = "MethodBasis", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 255)]
    public string? MethodBasis { get; set; }
    /// <summary>
    /// cutoff值
    /// </summary>
    /// <remarks>cutoff值</remarks>
    [SugarColumn(ColumnName = "CutOffValue", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? CutOffValue { get; set; }
    /// <summary>
    /// od值
    /// </summary>
    /// <remarks>od值</remarks>
    [SugarColumn(ColumnName = "ODValue", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ODValue { get; set; }
    /// <summary>
    /// s/co
    /// </summary>
    /// <remarks>s/co</remarks>
    [SugarColumn(ColumnName = "SpecificValue", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 255)]
    public string? SpecificValue { get; set; }
    /// <summary>
    /// 结果类型，定量、定性等
    /// </summary>
    /// <remarks>结果类型，定量、定性等</remarks>
    [SugarColumn(ColumnName = "ResultType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? ResultType { get; set; }
    /// <summary>
    /// 是否计算项0否1是
    /// </summary>
    /// <remarks>是否计算项0否1是</remarks>
    [SugarColumn(ColumnName = "IsCalculate", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool? IsCalculate { get; set; }
    /// <summary>
    /// 通用计算公式
    /// </summary>
    /// <remarks>通用计算公式</remarks>
    [SugarColumn(ColumnName = "CalcExpression", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 255)]
    public string? CalcExpression { get; set; }
    /// <summary>
    /// 上机次数
    /// </summary>
    /// <remarks>上机次数</remarks>
    [SugarColumn(ColumnName = "InTestCount", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? InTestCount { get; set; }
    /// <summary>
    /// 委托状态
    /// </summary>
    /// <remarks>委托状态</remarks>
    [SugarColumn(ColumnName = "EntrustStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? EntrustStatus { get; set; }
    /// <summary>
    /// 增项
    /// </summary>
    /// <remarks>增项</remarks>
    [SugarColumn(ColumnName = "AddType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? AddType { get; set; }
    /// <summary>
    /// 复查
    /// </summary>
    /// <remarks>复查</remarks>
    [SugarColumn(ColumnName = "ReviewStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "0", DecimalDigits = 11)]
    public int? ReviewStatus { get; set; } = 0;
}

#pragma warning restore CS8618

