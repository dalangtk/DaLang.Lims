using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Shared.Domain.ExamTaskDetail;

/// <summary>
/// 检验任务明细 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "exam_task_detail")]
public partial class ExamTaskDetailEntity : EntityTenant
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
    /// 任务Id
    /// </summary>
    /// <remarks>任务Id</remarks>
    [SugarColumn(ColumnName = "TaskId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long TaskId { get; set; }
    /// <summary>
    /// 申请项目Id
    /// </summary>
    /// <remarks>申请项目Id</remarks>
    [SugarColumn(ColumnName = "ApplyItemId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long ApplyItemId { get; set; }
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
    public string? PurCode { get; set; }
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
    /// 接入检验
    /// </summary>
    /// <remarks>接入检验</remarks>
    [SugarColumn(ColumnName = "InTest", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DefaultValue = "0")]
    public int InTest { get; set; } = 0;
}

#pragma warning restore CS8618

