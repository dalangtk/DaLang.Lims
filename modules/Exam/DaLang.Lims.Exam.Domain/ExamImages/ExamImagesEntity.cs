using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Exam.Domain.ExamImages;

/// <summary>
/// 检验图片 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "exam_images")]
public partial class ExamImagesEntity : EntityTenant
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamInfoId", ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? ExamInfoId { get; set; }
    /// <summary>
    /// 文件名
    /// </summary>
    [SugarColumn(ColumnName = "FileName", ColumnDataType = "varchar", Length = 64)]
    public string? FileName { get; set; }
    /// <summary>
    /// 文件地址
    /// </summary>
    /// <remarks>文件地址</remarks>
    [SugarColumn(ColumnName = "FileUrl", ColumnDataType = "varchar", Length = 255)]
    public string? FileUrl { get; set; }
    /// <summary>
    /// 缩放代码
    /// </summary>
    /// <remarks>缩放代码</remarks>
    [SugarColumn(ColumnName = "ZoomCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? ZoomCode { get; set; }
    /// <summary>
    /// 缩放名称
    /// </summary>
    /// <remarks>缩放名称</remarks>
    [SugarColumn(ColumnName = "ZoomName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? ZoomName { get; set; }
    /// <summary>
    /// 抗体代码
    /// </summary>
    /// <remarks>抗体代码</remarks>
    [SugarColumn(ColumnName = "AntiBodyCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 8)]
    public string? AntiBodyCode { get; set; }
    /// <summary>
    /// 抗体名称
    /// </summary>
    /// <remarks>抗体名称</remarks>
    [SugarColumn(ColumnName = "AntiBodyName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? AntiBodyName { get; set; }
    /// <summary>
    /// 图片类型 0常规 1巨检
    /// </summary>
    /// <remarks>图片类型 0常规 1巨检</remarks>
    [SugarColumn(ColumnName = "ImageType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 2)]
    public int ImageType { get; set; }
    /// <summary>
    /// 是否显示
    /// </summary>
    /// <remarks>是否显示</remarks>
    [SugarColumn(ColumnName = "IsShow", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "1")]
    public bool IsShow { get; set; } = true;
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int Sort { get; set; }

}

#pragma warning restore CS8618

