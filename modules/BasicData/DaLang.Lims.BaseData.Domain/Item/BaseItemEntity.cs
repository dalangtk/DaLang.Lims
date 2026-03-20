using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618

namespace DaLang.Lims.BaseData.Domain.Item;

/// <summary>
/// 基础项目实体类
/// </summary>
/// <remarks></remarks>
[SugarTable(TableName = "base_item")]
public class BaseItemEntity : EntityBase
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
    /// 项目代码
    /// </summary>
    /// <remarks>项目代码</remarks>
    [SugarColumn(ColumnName = "ItemCode", ColumnDataType = "varchar", Length = 16)]
    public string ItemCode { get; set; }
    /// <summary>
    /// 项目名称
    /// </summary>
    /// <remarks>项目名称</remarks>
    [SugarColumn(ColumnName = "ItemName", ColumnDataType = "varchar", Length = 32)]
    public string ItemName { get; set; }
    /// <summary>
    /// 结果类型，定量、定性等
    /// </summary>
    [SugarColumn(ColumnName = "ResultType", ColumnDataType = "varchar", Length = 8)]
    public string? ResultType { get; set; }
    /// <summary>
    /// 结果判定方式，高低/阴阳性/不判定
    /// </summary>
    [SugarColumn(ColumnName = "DecideType", ColumnDataType = "varchar", Length = 8)]
    public string? DecideType { get; set; }
    /// <summary>
    /// 缩写
    /// </summary>
    [SugarColumn(ColumnName = "ItemNameAB", ColumnDataType = "varchar", Length = 16)]
    public string? ItemNameAB { get; set; }
    /// <summary>
    /// 英文
    /// </summary>
    [SugarColumn(ColumnName = "ItemNameEN", ColumnDataType = "varchar", Length = 32)]
    public string? ItemNameEN { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    /// <remarks>排序</remarks>
    [SugarColumn(ColumnName = "Sort")]
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    /// <remarks>启用</remarks>
    [SugarColumn(ColumnName = "IsValid")]
    public bool IsValid { get; set; } = true;


    [Navigate(NavigateType.OneToOne, nameof(ItemCode), nameof(BaseItemPersonalizeEntity.ItemCode))]
    public BaseItemPersonalizeEntity ItemPersonal { get; set; }
}

#pragma warning restore CS8618

