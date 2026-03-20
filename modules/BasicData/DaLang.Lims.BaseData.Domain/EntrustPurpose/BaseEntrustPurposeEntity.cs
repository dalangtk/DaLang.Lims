using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Web.BaseData.Domain.EntrustPurpose;

/// <summary>
/// 委托目的 实体类
/// </summary>
/// <remarks>委托目的</remarks>
[SugarTable(TableName = "base_entrust_purpose")]
public partial class BaseEntrustPurposeEntity : EntityTenant
{
    /// <summary>
    /// 目的代码
    /// </summary>
    /// <remarks>目的代码</remarks>
    [SugarColumn(ColumnName = "PurCode", ColumnDataType = "varchar", Length = 16)]
    public string PurCode { get; set; }
    /// <summary>
    /// 问询规则
    /// </summary>
    /// <remarks>问询规则</remarks>
    [SugarColumn(ColumnName = "AskRuleCode", ColumnDataType = "varchar", Length = 16)]
    public string? AskRuleCode { get; set; }
    /// <summary>
    /// 起始时间
    /// </summary>
    /// <remarks>起始时间</remarks>
    [SugarColumn(ColumnName = "BeginTime", ColumnDataType = "datetime")]
    public DateTime? BeginTime { get; set; }
    /// <summary>
    /// 截止时间
    /// </summary>
    /// <remarks>截止时间</remarks>
    [SugarColumn(ColumnName = "EndTime", ColumnDataType = "datetime")]
    public DateTime? EndTime { get; set; }
    /// <summary>
    /// 接收周期
    /// </summary>
    /// <remarks>接收周期</remarks>
    [SugarColumn(ColumnName = "ReceiveDays", ColumnDataType = "varchar", Length = 16)]
    public string? ReceiveDays { get; set; }
    /// <summary>
    /// 接收时间
    /// </summary>
    /// <remarks>接收时间</remarks>
    [SugarColumn(ColumnName = "ReceiveTime", ColumnDataType = "varchar", Length = 16)]
    public string? ReceiveTime { get; set; }
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", ColumnDataType = "varchar", Length = 8)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 标本类型
    /// </summary>
    /// <remarks>标本类型</remarks>
    [SugarColumn(ColumnName = "SampleTypeCode", ColumnDataType = "varchar", Length = 16)]
    public string? SampleTypeCode { get; set; }
    /// <summary>
    /// 客户反向判定
    /// </summary>
    /// <remarks>客户反向判定</remarks>
    [SugarColumn(ColumnName = "IsCustomerReverse", ColumnDataType = "tinyint")]
    public bool? IsCustomerReverse { get; set; }
    /// <summary>
    /// 委托医院代码
    /// </summary>
    /// <remarks>委托医院代码</remarks>
    [SugarColumn(ColumnName = "EntrustHospitalCode", ColumnDataType = "varchar", Length = 32)]
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    /// 委托医院名称
    /// </summary>
    /// <remarks>委托医院名称</remarks>
    [SugarColumn(ColumnName = "EntrustHospitalName", ColumnDataType = "varchar", Length = 64)]
    public string? EntrustHospitalName { get; set; }
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
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }
}

#pragma warning restore CS8618

