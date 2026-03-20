using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.Web.Framework.Domain.ModifledLog;

#pragma warning disable CS8618
/// <summary>
/// 修改记录 实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "sys_modified_log")]
public partial class ModifiedLogEntity : EntityTenant
{
    /// <summary>
    /// 表名
    /// </summary>
    /// <remarks>表名</remarks>
    [SugarColumn(ColumnName = "TableName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? TableName { get; set; }
    /// <summary>
    /// 数据id
    /// </summary>
    /// <remarks>数据id</remarks>
    [SugarColumn(ColumnName = "DataId", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 20)]
    public string? DataId { get; set; }
    /// <summary>
    /// 字段名
    /// </summary>
    /// <remarks>字段名</remarks>
    [SugarColumn(ColumnName = "FieldName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 32)]
    public string? FieldName { get; set; }
    /// <summary>
    /// 原始值
    /// </summary>
    /// <remarks>原始值</remarks>
    [SugarColumn(ColumnName = "OriginalValue", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? OriginalValue { get; set; }
    /// <summary>
    /// 新值
    /// </summary>
    /// <remarks>新值</remarks>
    [SugarColumn(ColumnName = "NewValue", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 256)]
    public string? NewValue { get; set; }
    /// <summary>
    /// 操作类型
    /// </summary>
    /// <remarks>操作类型</remarks>
    [SugarColumn(ColumnName = "ModifyType", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "int", DecimalDigits = 11)]
    public int? ModifyType { get; set; }
}
#pragma warning restore CS8618