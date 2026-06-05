using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8618
namespace DaLang.Lims.Pathology.Domain.PathologySamplingSpotDetail;

/// <summary>
/// 取材部位明细 实体类
/// </summary>
/// <remarks>采样部位关联标本类型</remarks>
[SugarTable(TableName = "base_pathology_sampling_spot_detail")]
public partial class BasePathologySamplingSpotDetailEntity : EntityBase
{
    /// <summary>
    /// 采样部位代码
    /// </summary>
    /// <remarks>采样部位代码</remarks>
    [SugarColumn(ColumnName = "SamplingSpotCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SamplingSpotCode { get; set; }
    /// <summary>
    /// 标本类型代码
    /// </summary>
    /// <remarks>标本类型代码</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SampleTypeCode { get; set; }
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

