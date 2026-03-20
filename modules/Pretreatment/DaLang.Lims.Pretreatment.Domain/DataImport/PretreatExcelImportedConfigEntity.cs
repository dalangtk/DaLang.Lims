using System;
using SqlSugar;
using DaLang.Lims.Web.Framework.Core.Entities;

#pragma warning disable CS8618
namespace DaLang.Lims.Pretreatment.Domain.PretreatDataImport;

/// <summary>
/// 已导入文件的数据对照设置实体类
/// </summary>
/// <remarks>已导入文件的数据对照设置</remarks>
[SugarTable(TableName = "pretreat_excel_imported_config")]
public partial class PretreatExcelImportedConfigEntity : EntityTenant
{
    /// <summary>
    /// 文件id
    /// </summary>
    /// <remarks>文件id</remarks>
    [SugarColumn(ColumnName = "FileName", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "bigint", DecimalDigits = 20)]
    public long? FileName { get; set; }
    /// <summary>
    /// 导入配置json
    /// </summary>
    /// <remarks>导入配置json</remarks>
    [SugarColumn(ColumnName = "ConfigJson", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "text", Length = 0)]
    public string? ConfigJson { get; set; }
}

#pragma warning restore CS8618

