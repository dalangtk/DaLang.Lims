using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

namespace DaLang.Lims.BaseData.Domain.Item;
#pragma warning disable CS8618

/// <summary>
/// 项目个性化实体类
/// </summary>
[SugarTable(TableName = "base_item_personalize")]
public class BaseItemPersonalizeEntity : EntityTenant
{
    /// <summary>
    /// 项目代码
    /// </summary>
    /// <remarks>项目代码</remarks>
    [SugarColumn(ColumnName = "ItemCode", ColumnDataType = "varchar", Length = 16)]
    public string ItemCode { get; set; }
    /// <summary>
    /// 个性化项目名称
    /// </summary>
    /// <remarks>个性化项目名称</remarks>
    [SugarColumn(ColumnName = "ItemNamePersonalize", ColumnDataType = "varchar", Length = 32)]
    public string? ItemNamePersonalize { get; set; }
    /// <summary>
    /// 是否计算项0否1是
    /// </summary>
    /// <remarks>是否计算项0否1是</remarks>
    [SugarColumn(ColumnName = "IsCalculcate", ColumnDataType = "tinyint")]
    public bool IsCalculcate { get; set; } = false;
    /// <summary>
    /// 通用计算公式
    /// </summary>
    /// <remarks>通用计算公式</remarks>
    [SugarColumn(ColumnName = "CalcExpression", ColumnDataType = "varchar", Length = 255)]
    public string? CalcExpression { get; set; }
    /// <summary>
    /// 方法学
    /// </summary>
    /// <remarks>方法学</remarks>
    [SugarColumn(ColumnName = "MethodCode", ColumnDataType = "varchar", Length = 32)]
    public string? MethodCode { get; set; }
    /// <summary>
    /// 项目单位
    /// </summary>
    /// <remarks>项目单位</remarks>
    [SugarColumn(ColumnName = "ItemUnit", ColumnDataType = "varchar", Length = 32)]
    public string? ItemUnit { get; set; }
    /// <summary>
    /// 结果精度
    /// </summary>
    /// <remarks>结果精度</remarks>
    [SugarColumn(ColumnName = "ResultAccuracy", IsOnlyIgnoreInsert = false, IsOnlyIgnoreUpdate = false, ColumnDataType = "tinyint")]
    public int? ResultAccuracy { get; set; } = 0;
    /// <summary>
    /// 默认结果
    /// </summary>
    /// <remarks>默认结果</remarks>
    [SugarColumn(ColumnName = "DefaultValue", ColumnDataType = "varchar", Length = 64)]
    public string? DefaultValue { get; set; }
    /// <summary>
    /// 结果调整系数
    /// </summary>
    /// <remarks>结果调整系数</remarks>
    [SugarColumn(ColumnName = "ResultCoefficient", ColumnDataType = "varchar", Length = 32)]
    public string? ResultCoefficient { get; set; }
    /// <summary>
    /// 报告显示
    /// </summary>
    /// <remarks>报告显示</remarks>
    [SugarColumn(ColumnName = "IsReportShow", ColumnDataType = "tinyint")]
    public bool IsReportShow { get; set; } = true;
    /// <summary>
    /// 结果复核
    /// </summary>
    /// <remarks>结果复核</remarks>
    [SugarColumn(ColumnName = "IsReviewResult", ColumnDataType = "tinyint")]
    public bool? IsReviewResult { get; set; }
    /// <summary>
    /// 必须参考范围
    /// </summary>
    /// <remarks>必须参考范围</remarks>
    [SugarColumn(ColumnName = "IsMustReference", ColumnDataType = "tinyint")]
    public bool? IsMustReference { get; set; }
    /// <summary>
    /// 打印排序
    /// </summary>
    /// <remarks>打印排序</remarks>
    [SugarColumn(ColumnName = "PrintOrder", ColumnDataType = "varchar", Length = 16)]
    public string? PrintOrder { get; set; }
    /// <summary>
    /// 方法依据
    /// </summary>
    /// <remarks>方法依据</remarks>
    [SugarColumn(ColumnName = "MethodBasis", ColumnDataType = "varchar", Length = 255)]
    public string? MethodBasis { get; set; }
    /// <summary>
    /// 项目统计类别
    /// </summary>
    /// <remarks>项目统计类别</remarks>
    [SugarColumn(ColumnName = "ItemStaticType", ColumnDataType = "varchar", Length = 8)]
    public string? ItemStaticType { get; set; }
    /// <summary>
    /// 项目系列
    /// </summary>
    /// <remarks>项目系列</remarks>
    [SugarColumn(ColumnName = "ItemSeriesType", ColumnDataType = "varchar", Length = 8)]
    public string? ItemSeriesType { get; set; }
    /// <summary>
    /// 拼音码
    /// </summary>
    /// <remarks>拼音码</remarks>
    [SugarColumn(ColumnName = "PinYin", ColumnDataType = "varchar", Length = 32)]
    public string? PinYin { get; set; }
    /// <summary>
    /// 五笔码
    /// </summary>
    /// <remarks>五笔码</remarks>
    [SugarColumn(ColumnName = "WuBi", ColumnDataType = "varchar", Length = 32)]
    public string? WuBi { get; set; }
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