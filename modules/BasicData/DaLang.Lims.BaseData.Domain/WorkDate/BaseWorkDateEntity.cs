using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Web.BaseData.Domain.BaseWorkDate;

/// <summary>
/// 工作日 实体类
/// </summary>
/// <remarks>工作日</remarks>
[SugarTable(TableName = "base_work_date")]
public partial class BaseWorkDateEntity : EntityTenant
{
    /// <summary>
    /// 日期
    /// </summary>
    /// <remarks>日期</remarks>
    [SugarColumn(ColumnName = "WorkDate", ColumnDataType = "datetime")]
    public DateTime? WorkDate { get; set; }
    /// <summary>
    /// 年
    /// </summary>
    /// <remarks>年</remarks>
    [SugarColumn(ColumnName = "Year", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? Year { get; set; }
    /// <summary>
    /// 月
    /// </summary>
    /// <remarks>月</remarks>
    [SugarColumn(ColumnName = "Month", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? Month { get; set; }
    /// <summary>
    /// 日
    /// </summary>
    /// <remarks>日</remarks>
    [SugarColumn(ColumnName = "Day", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? Day { get; set; }
    /// <summary>
    /// 日期类型
    /// </summary>
    /// <remarks>日期类型</remarks>
    [SugarColumn(ColumnName = "DateType", ColumnDataType = "int", DecimalDigits = 11)]
    public int? DateType { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }
}

#pragma warning restore CS8618

