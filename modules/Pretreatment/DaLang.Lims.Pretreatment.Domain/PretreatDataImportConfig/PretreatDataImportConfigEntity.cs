using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.PretreatDataImportConfig;

/// <summary>
/// 导入配置 实体类
/// </summary>
/// <remarks>数据导入配置</remarks>
[SugarTable(TableName = "pretreat_data_import_config")]
public partial class PretreatDataImportConfigEntity : EntityTenant
{
    /// <summary>
    /// 客户代码
    /// </summary>
    /// <remarks>客户代码</remarks>
    [SugarColumn(ColumnName = "CustomerCode", ColumnDataType = "varchar", Length = 8)]
    public string? CustomerCode { get; set; }
    /// <summary>
    /// 列名
    /// </summary>
    /// <remarks>列名</remarks>
    [SugarColumn(ColumnName = "CellName", ColumnDataType = "varchar", Length = 32)]
    public string? CellName { get; set; }
    /// <summary>
    /// 字段名
    /// </summary>
    /// <remarks>字段名</remarks>
    [SugarColumn(ColumnName = "FieldName", ColumnDataType = "varchar", Length = 32)]
    public string? FieldName { get; set; }
    /// <summary>
    /// 转换函数
    /// </summary>
    /// <remarks>转换函数</remarks>
    [SugarColumn(ColumnName = "TranslateFunction", ColumnDataType = "varchar", Length = 64)]
    public string? TranslateFunction { get; set; }
    /// <summary>
    /// 必须存在
    /// </summary>
    /// <remarks></remarks>
    [SugarColumn(ColumnName = "IsMustExists", ColumnDataType = "tinyint")]
    public bool IsMustExists { get; set; } = false;
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

