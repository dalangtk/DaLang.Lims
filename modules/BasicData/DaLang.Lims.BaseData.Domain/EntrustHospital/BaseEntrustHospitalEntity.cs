using DaLang.Lims.Web.Framework.Core.Attributes;
using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.EntrustHospital;

/// <summary>
/// 委托医院 实体类
/// </summary>
/// <remarks>委托医院</remarks>
[SugarTable(TableName = "base_entrust_hospital")]
public partial class BaseEntrustHospitalEntity : EntityBase
{
    /// <summary>
    /// 委托医院代码
    /// </summary>
    /// <remarks>委托医院代码</remarks>
    [SugarColumn(ColumnName = "EntrustHospitalCode", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? EntrustHospitalCode { get; set; }
    /// <summary>
    /// 委托医院名称
    /// </summary>
    /// <remarks>委托医院名称</remarks>
    [SugarColumn(ColumnName = "EntrustHospitalName", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? EntrustHospitalName { get; set; }
    /// <summary>
    /// 联系人
    /// </summary>
    /// <remarks>联系人</remarks>
    [SugarColumn(ColumnName = "Contacts", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? Contacts { get; set; }
    /// <summary>
    /// 联系电话
    /// </summary>
    /// <remarks>联系电话</remarks>
    [SugarColumn(ColumnName = "ContactPhone", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? ContactPhone { get; set; }
    /// <summary>
    /// 拼音
    /// </summary>
    /// <remarks>拼音</remarks>
    [SugarColumn(ColumnName = "PinYin", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? PinYin { get; set; }
    /// <summary>
    /// 五笔
    /// </summary>
    /// <remarks>五笔</remarks>
    [SugarColumn(ColumnName = "WuBi", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? WuBi { get; set; }
    /// <summary>
    /// 自定义码
    /// </summary>
    /// <remarks>自定义码</remarks>
    [SugarColumn(ColumnName = "CustomCode", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? CustomCode { get; set; }
    /// <summary>
    /// 备注
    /// </summary>
    /// <remarks>备注</remarks>
    [SugarColumn(ColumnName = "Remark", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? Remark { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort", IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    [SortGrowStep(10)]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public bool IsValid { get; set; }
}

#pragma warning restore CS8618

