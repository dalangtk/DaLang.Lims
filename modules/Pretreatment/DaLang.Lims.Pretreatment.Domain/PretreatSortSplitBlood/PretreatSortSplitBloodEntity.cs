using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.PretreatSortSplitBlood;

/// <summary>
/// 标本分血实体类
/// </summary>
/// <remarks>分血表</remarks>
[SugarTable(TableName = "pretreat_sort_split_blood")]
public partial class PretreatSortSplitBloodEntity : EntityTenant
{
    /// <summary>
    /// 条码
    /// </summary>
    /// <remarks>条码</remarks>
    [SugarColumn(ColumnName = "Barcode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string Barcode { get; set; }
    /// <summary>
    /// 标本类型代码
    /// </summary>
    /// <remarks>标本类型代码</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 标本类型名称
    /// </summary>
    /// <remarks>标本类型名称</remarks>
    [SugarColumn(ColumnName = "SampleTypeName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SampleTypeName { get; set; }
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
    /// 接收时间
    /// </summary>
    /// <remarks>接收时间</remarks>
    [SugarColumn(ColumnName = "ReceiveTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? ReceiveTime { get; set; }
    /// <summary>
    /// 分血管数
    /// </summary>
    /// <remarks>分血管数</remarks>
    [SugarColumn(ColumnName = "SplitTubeCnt", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? SplitTubeCnt { get; set; }
    /// <summary>
    /// 分血状态 0未分血 1已分血
    /// </summary>
    /// <remarks>分血状态</remarks>
    [SugarColumn(ColumnName = "SplitBloodStatus", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public int? SplitBloodStatus { get; set; }
    /// <summary>
    /// 分血人id
    /// </summary>
    /// <remarks>分血人id</remarks>
    [SugarColumn(ColumnName = "SplitBloodUserId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? SplitBloodUserId { get; set; }
    /// <summary>
    /// 分血人姓名
    /// </summary>
    /// <remarks>分血人姓名</remarks>
    [SugarColumn(ColumnName = "SplitBloodUserName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? SplitBloodUserName { get; set; }
    /// <summary>
    /// 分血时间
    /// </summary>
    /// <remarks>分血时间</remarks>
    [SugarColumn(ColumnName = "SplitBloodTime", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "datetime")]
    public DateTime? SplitBloodTime { get; set; }
}

#pragma warning restore CS8618

