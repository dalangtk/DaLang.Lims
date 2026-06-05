using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.PathologyDiseaseDetail;

/// <summary>
/// 疾病明细 实体类
/// </summary>
/// <remarks>疾病明细</remarks>
[SugarTable(TableName = "base_pathology_disease_detail")]
public partial class BasePathologyDiseaseDetailEntity : EntityBase
{
    /// <summary>
    /// 疾病代码
    /// </summary>
    /// <remarks>疾病代码</remarks>
    [SugarColumn(ColumnName = "DiseaseCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? DiseaseCode { get; set; }
    /// <summary>
    /// 子疾病代码
    /// </summary>
    /// <remarks>子疾病代码</remarks>
    [SugarColumn(ColumnName = "SubDiseaseCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SubDiseaseCode { get; set; }
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
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

