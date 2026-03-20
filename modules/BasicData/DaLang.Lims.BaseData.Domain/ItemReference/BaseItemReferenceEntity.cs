using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618

namespace DaLang.Lims.BaseData.Domain.ItemReference;

/// <summary>
/// 项目参考范围实体类
/// </summary>
/// <remarks>参考范围</remarks>
[SugarTable(TableName = "base_item_reference")]
public partial class BaseItemReferenceEntity : EntityTenant
{
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", ColumnDataType = "varchar", Length = 16)]
    public string GroupCode { get; set; }
    /// <summary>
    /// 项目代码
    /// </summary>
    /// <remarks>项目代码</remarks>
    [SugarColumn(ColumnName = "ItemCode", ColumnDataType = "varchar", Length = 16)]
    public string ItemCode { get; set; }
    /// <summary>
    /// 结果类型，定量、定性等
    /// </summary>
    /// <remarks>结果类型，定量、定性等</remarks>
    [SugarColumn(ColumnName = "ResultType", ColumnDataType = "varchar", Length = 8)]
    public string? ResultType { get; set; }
    /// <summary>
    /// 方法学
    /// </summary>
    /// <remarks>方法学</remarks>
    [SugarColumn(ColumnName = "Method", ColumnDataType = "varchar", Length = 8)]
    public string? Method { get; set; }
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", ColumnDataType = "varchar", Length = 16)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 仪器
    /// </summary>
    /// <remarks>仪器</remarks>
    [SugarColumn(ColumnName = "InstrumentCode", ColumnDataType = "varchar", Length = 16)]
    public string? InstrumentCode { get; set; }
    /// <summary>
    /// 试剂
    /// </summary>
    /// <remarks>试剂</remarks>
    [SugarColumn(ColumnName = "ReagentCode", ColumnDataType = "varchar", Length = 16)]
    public string? ReagentCode { get; set; }
    /// <summary>
    /// 标本类型
    /// </summary>
    /// <remarks>标本类型</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", ColumnDataType = "varchar", Length = 16)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 性别
    /// </summary>
    /// <remarks>性别</remarks>
    [SugarColumn(ColumnName = "GenderCode", ColumnDataType = "varchar", Length = 8)]
    public string? GenderCode { get; set; }
    /// <summary>
    /// 委托医院
    /// </summary>
    /// <remarks>委托医院</remarks>
    [SugarColumn(ColumnName = "EntrustHospitalCode", ColumnDataType = "varchar", Length = 8)]
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    /// 年龄下限
    /// </summary>
    /// <remarks>年龄下限</remarks>
    [SugarColumn(ColumnName = "AgeLowLimit", ColumnDataType = "int", DecimalDigits = 4)]
    public int? AgeLowLimit { get; set; }
    /// <summary>
    /// 年龄下限 分钟
    /// </summary>
    /// <remarks>年龄下限 分钟</remarks>
    [SugarColumn(ColumnName = "AgeLowValue", ColumnDataType = "int", DecimalDigits = 12)]
    public int? AgeLowValue { get; set; }
    /// <summary>
    /// 年龄上限
    /// </summary>
    /// <remarks>年龄上限</remarks>
    [SugarColumn(ColumnName = "AgeUpperLimit", ColumnDataType = "int", DecimalDigits = 4)]
    public int? AgeUpperLimit { get; set; }
    /// <summary>
    /// 年龄上限 分钟
    /// </summary>
    /// <remarks>年龄上限 分钟</remarks>
    [SugarColumn(ColumnName = "AgeUpperValue", ColumnDataType = "int", DecimalDigits = 12)]
    public int? AgeUpperValue { get; set; }
    /// <summary>
    /// 年龄单位
    /// </summary>
    /// <remarks>年龄单位</remarks>
    [SugarColumn(ColumnName = "AgeUnit", ColumnDataType = "varchar", Length = 8)]
    public string? AgeUnit { get; set; }
    /// <summary>
    /// 警告范围
    /// </summary>
    /// <remarks>警告范围</remarks>
    [SugarColumn(ColumnName = "WarningRange", ColumnDataType = "varchar", Length = 64)]
    public string? WarningRange { get; set; }
    /// <summary>
    /// 参考范围
    /// </summary>
    /// <remarks>参考范围</remarks>
    [SugarColumn(ColumnName = "ReferenceRange", ColumnDataType = "varchar", Length = 64)]
    public string? ReferenceRange { get; set; }
    /// <summary>
    /// 危急值范围
    /// </summary>
    /// <remarks>危急值范围</remarks>
    [SugarColumn(ColumnName = "CriticalRange", ColumnDataType = "varchar", Length = 64)]
    public string? CriticalRange { get; set; }
    /// <summary>
    /// 报告显示范围
    /// </summary>
    /// <remarks>报告显示范围</remarks>
    [SugarColumn(ColumnName = "DisplayRange", ColumnDataType = "varchar", Length = 128)]
    public string? DisplayRange { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", ColumnDataType = "int", DecimalDigits = 11)]
    [SortGrowStep(10)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }
}

#pragma warning restore CS8618

