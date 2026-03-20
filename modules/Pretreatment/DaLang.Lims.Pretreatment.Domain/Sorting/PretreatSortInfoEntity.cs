using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.Sorting;

/// <summary>
/// 基础数据 实体类
/// </summary>
/// <remarks>分拣信息</remarks>
[SugarTable(TableName = "pretreat_sort_info")]
public partial class PretreatSortInfoEntity : EntityTenant
{
    /// <summary>
    /// 分拣代码
    /// </summary>
    /// <remarks>分拣代码</remarks>
    [SugarColumn(ColumnName = "SortInfoCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? SortInfoCode { get; set; }
    /// <summary>
    /// 分拣仪代码
    /// </summary>
    /// <remarks>分拣仪代码</remarks>
    [SugarColumn(ColumnName = "SorterCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? SorterCode { get; set; }
    /// <summary>
    /// 分拣开始时间
    /// </summary>
    /// <remarks>分拣开始时间</remarks>
    [SugarColumn(ColumnName = "StartTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? StartTime { get; set; }
    /// <summary>
    /// 分拣结束时间
    /// </summary>
    /// <remarks>分拣结束时间</remarks>
    [SugarColumn(ColumnName = "EndTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 上架模式0否 1是
    /// </summary>
    [SugarColumn(ColumnName = "IsShelfMode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public int? IsShelfMode { get; set; } = 0;
    /// <summary>
    /// 分拣状态 0已结束
    /// </summary>
    /// <remarks>分拣状态</remarks>
    [SugarColumn(ColumnName = "Status", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? Status { get; set; }
    /// <summary>
    /// 主机名
    /// </summary>
    /// <remarks>主机名</remarks>
    [SugarColumn(ColumnName = "HostName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? HostName { get; set; }
}

#pragma warning restore CS8618

