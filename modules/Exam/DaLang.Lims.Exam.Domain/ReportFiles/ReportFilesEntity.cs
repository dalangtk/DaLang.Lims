using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Exam.Domain.ReportFiles;

/// <summary>
/// 报告 实体类
/// </summary>
/// <remarks>报告文件</remarks>
[SugarTable(TableName = "report_files")]
public partial class ReportFilesEntity : EntityTenant
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "ExamInfoId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long ExamInfoId { get; set; }
    /// <summary>
    /// 报告地址
    /// </summary>
    /// <remarks>报告地址</remarks>
    [SugarColumn(ColumnName = "FilePath", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 128)]
    public string? FilePath { get; set; }
    /// <summary>
    /// 文件名
    /// </summary>
    /// <remarks>文件名</remarks>
    [SugarColumn(ColumnName = "FileName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? FileName { get; set; }
    /// <summary>
    /// 纸张类型 0 A5 1 A4
    /// </summary>
    /// <remarks>纸张类型</remarks>
    [SugarColumn(ColumnName = "PaperType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 4)]
    public int PaperType { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "1")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

