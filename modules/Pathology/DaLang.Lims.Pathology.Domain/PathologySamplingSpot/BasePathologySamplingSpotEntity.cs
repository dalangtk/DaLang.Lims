using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.PathologySamplingSpot;

/// <summary>
/// 取材部位 实体类
/// </summary>
/// <remarks>取材部位</remarks>
[SugarTable(TableName = "base_pathology_sampling_spot")]
public partial class BasePathologySamplingSpotEntity : EntityBase
{
    /// <summary>
    /// 取材部位代码
    /// </summary>
    /// <remarks>取材部位代码</remarks>
    [SugarColumn(ColumnName = "SamplingSpotCode", ColumnDataType = "varchar", Length = 16)]
    public string? SamplingSpotCode { get; set; }
    /// <summary>
    /// 取材部位名称
    /// </summary>
    /// <remarks>取材部位名称</remarks>
    [SugarColumn(ColumnName = "SamplingSpotName", ColumnDataType = "varchar", Length = 64)]
    public string? SamplingSpotName { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    /// <remarks>性别</remarks>
    [SugarColumn(ColumnName = "Gender", ColumnDataType = "varchar", Length = 8)]
    public string? Gender { get; set; }
    /// <summary>
    /// 拼音
    /// </summary>
    /// <remarks>拼音</remarks>
    [SugarColumn(ColumnName = "PinYin", ColumnDataType = "varchar", Length = 16)]
    public string? PinYin { get; set; }
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
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint", DefaultValue = "true")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

