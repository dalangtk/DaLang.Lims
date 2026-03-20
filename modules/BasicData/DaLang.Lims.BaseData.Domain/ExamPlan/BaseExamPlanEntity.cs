using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.ExamPlan;

/// <summary>
/// 检测计划 实体类
/// </summary>
/// <remarks>检测计划</remarks>
[SugarTable(TableName = "base_exam_plan")]
public partial class BaseExamPlanEntity : EntityTenant
{
    /// <summary>
    /// 检测计划代码
    /// </summary>
    /// <remarks>检测计划代码</remarks>
    [SugarColumn(ColumnName = "ExamPlanCode", ColumnDataType = "varchar", Length = 8)]
    public string ExamPlanCode { get; set; }
    /// <summary>
    /// 检测计划名称
    /// </summary>
    /// <remarks>检测计划名称</remarks>
    [SugarColumn(ColumnName = "ExamPlanName", ColumnDataType = "varchar", Length = 64)]
    public string ExamPlanName { get; set; }
    /// <summary>
    /// 计划类型
    /// </summary>
    /// <remarks>计划类型</remarks>
    [SugarColumn(ColumnName = "PlanType", ColumnDataType = "varchar", Length = 8)]
    public string PlanType { get; set; }
    /// <summary>
    /// 检测周期
    /// </summary>
    /// <remarks>检测周期</remarks>
    [SugarColumn(ColumnName = "PlanCycle", ColumnDataType = "varchar", Length = 16)]
    public string? PlanCycle { get; set; }
    /// <summary>
    /// 包含节假日
    /// </summary>
    /// <remarks>包含节假日</remarks>
    [SugarColumn(ColumnName = "IsIncludeHoliday", ColumnDataType = "tinyint")]
    public bool IsIncludeHoliday { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
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

