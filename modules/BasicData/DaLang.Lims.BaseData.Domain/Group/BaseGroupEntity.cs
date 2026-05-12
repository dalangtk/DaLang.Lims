using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618

namespace DaLang.Lims.BaseData.Domain.Group;

/// <summary>
/// 基础组别实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "base_group")]
public partial class BaseGroupEntity : EntityBase
{
    /// <summary>
    /// 组别代码
    /// </summary>
    /// <remarks>组别代码</remarks>
    [SugarColumn(ColumnName = "GroupCode", ColumnDataType = "varchar", Length = 16)]
    public string GroupCode { get; set; }
    /// <summary>
    /// 组别名称
    /// </summary>
    /// <remarks>组别名称</remarks>
    [SugarColumn(ColumnName = "GroupName", ColumnDataType = "varchar", Length = 32)]
    public string GroupName { get; set; }
    /// <summary>
    /// 父级组别
    /// </summary>
    /// <remarks>父级组别</remarks>
    [SugarColumn(ColumnName = "ParentCode", ColumnDataType = "varchar", Length = 16)]
    public string? ParentCode { get; set; }
    /// <summary>
    /// 排序
    /// <summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort")]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid", DefaultValue = "1")]
    public bool IsValid { get; set; } = true;
}

#pragma warning restore CS8618

