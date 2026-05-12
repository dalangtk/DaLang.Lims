using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.Shared.Domain.ExamSpecialResult;

#pragma warning disable CS8618
/// <summary>
/// 特检结果 实体类
/// </summary>
/// <remarks>病理和特检结果</remarks>
[SugarTable(TableName = "exam_special_result")]
public partial class ExamSpecialResultEntity : EntityTenant
{
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
    public string GroupCode { get; set; }
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
    public DateTime TestDate { get; set; }
    /// <summary>
    /// 字段
    /// </summary>
    /// <remarks>字段</remarks>
    [SugarColumn(ColumnName = "FieldCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string FieldCode { get; set; }
    /// <summary>
    /// 字段名
    /// </summary>
    /// <remarks>字段名</remarks>
    [SugarColumn(ColumnName = "FieldName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 64)]
    public string? FieldName { get; set; }
    /// <summary>
    /// 字段值
    /// </summary>
    /// <remarks>字段值</remarks>
    [SugarColumn(ColumnName = "FieldValue", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 1024)]
    public string? FieldValue { get; set; }
    /// <summary>
    /// 结果类型 1初诊 2复诊
    /// </summary>
    /// <remarks>结果类型 1初诊 2复诊</remarks>
    [SugarColumn(ColumnName = "ResultType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", Length = 2)]
    public int? ResultType { get; set; }
}
#pragma warning restore CS8618
