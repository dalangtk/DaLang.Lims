using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.BaseData.Domain.Purpose;

/// <summary>
/// 检验目的机构设置 实体类
/// </summary>
[SugarTable(TableName = "base_purpose_tenant_setting")]
public partial class BasePurposeTenantSettingEntity : EntityTenant
{
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string PurCode { get; set; }
    /// <summary>
    /// 是否启用
    /// </summary>
    /// <remarks>是否启用</remarks>
    [SugarColumn(ColumnName = "IsEnable", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool? IsEnable { get; set; }
    /// <summary>
    /// 录入隐藏
    /// </summary>
    /// <remarks>录入隐藏</remarks>
    [SugarColumn(ColumnName = "IsInputHide", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool? IsInputHide { get; set; }
    /// <summary>
    /// 工作流
    /// </summary>
    /// <remarks>工作流</remarks>
    [SugarColumn(ColumnName = "WorkFlowType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? WorkFlowType { get; set; }
    /// <summary>
    /// 检测计划
    /// </summary>
    /// <remarks>检测计划</remarks>
    [SugarColumn(ColumnName = "ExamPlan", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? ExamPlan { get; set; }
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
    /// 检查性别
    /// </summary>
    /// <remarks>检查性别</remarks>
    [SugarColumn(ColumnName = "TestSex", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? TestSex { get; set; }
    /// <summary>
    /// 存储条件
    /// </summary>
    /// <remarks>存储条件</remarks>
    [SugarColumn(ColumnName = "StorageCondition", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? StorageCondition { get; set; }
    /// <summary>
    /// 存储周期
    /// </summary>
    /// <remarks>存储周期</remarks>
    [SugarColumn(ColumnName = "StorageCycle", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? StorageCycle { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }
}
