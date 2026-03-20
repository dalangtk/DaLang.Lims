using DaLang.Lims.Web.Framework.Core.Entities;
using SqlSugar;

#pragma warning disable CS8618
namespace DaLang.Lims.BaseData.Domain.Sequence;

/// <summary>
/// 序列管理 实体类
/// </summary>
/// <remarks>序列</remarks>
[SugarTable(TableName = "base_sequence")]
public partial class BaseSequenceEntity : EntityTenant
{
    /// <summary>
    /// 序列代码
    /// </summary>
    /// <remarks>序列代码</remarks>
    [SugarColumn(ColumnName = "SequenceCode", IsOnlyIgnoreUpdate = false, ColumnDataType = "varchar", Length = 16)]
    public string? SequenceCode { get; set; }
    /// <summary>
    /// 序列名称
    /// </summary>
    /// <remarks>序列名称</remarks>
    [SugarColumn(ColumnName = "SequenceName", ColumnDataType = "varchar", Length = 32)]
    public string? SequenceName { get; set; }
    /// <summary>
    /// 序列标识
    /// </summary>
    /// <remarks>序列标识</remarks>
    [SugarColumn(ColumnName = "Identifier", ColumnDataType = "varchar", Length = 4)]
    public string? Identifier { get; set; }
    /// <summary>
    /// 前缀
    /// </summary>
    /// <remarks>前缀</remarks>
    [SugarColumn(ColumnName = "Perfix", ColumnDataType = "varchar", Length = 32)]
    public string? Perfix { get; set; }
    /// <summary>
    /// 序列内容
    /// </summary>
    /// <remarks>序列内容</remarks>
    [SugarColumn(ColumnName = "SequenceValue", ColumnDataType = "varchar", Length = 16)]
    public string? SequenceValue { get; set; }
    /// <summary>
    /// 起始号
    /// </summary>
    /// <remarks>起始号</remarks>
    [SugarColumn(ColumnName = "StartNo", ColumnDataType = "int", DecimalDigits = 11)]
    public int? StartNo { get; set; }
    /// <summary>
    /// 步长
    /// </summary>
    /// <remarks>步长</remarks>
    [SugarColumn(ColumnName = "SequenceStep", ColumnDataType = "int", DecimalDigits = 11)]
    public int? SequenceStep { get; set; }
    /// <summary>
    /// 补齐长度
    /// </summary>
    /// <remarks>补齐长度</remarks>
    [SugarColumn(ColumnName = "CompletionLength", ColumnDataType = "int", DecimalDigits = 11)]
    public int? CompletionLength { get; set; }
    /// <summary>
    /// 补齐字符
    /// </summary>
    /// <remarks>补齐字符</remarks>
    [SugarColumn(ColumnName = "CompletionChar", ColumnDataType = "varchar", Length = 2)]
    public string? CompletionChar { get; set; }
    /// <summary>
    /// 后缀
    /// </summary>
    /// <remarks>后缀</remarks>
    [SugarColumn(ColumnName = "Suffix", ColumnDataType = "varchar", Length = 4)]
    public string? Suffix { get; set; }
    /// <summary>
    /// 重置类型
    /// </summary>
    /// <remarks>重置类型</remarks>
    [SugarColumn(ColumnName = "ResetType", ColumnDataType = "int", DecimalDigits = 11)]
    public int? ResetType { get; set; }
    /// <summary>
    /// 重置时间
    /// </summary>
    /// <remarks>重置时间</remarks>
    [SugarColumn(ColumnName = "ResetTime", ColumnDataType = "varchar", Length = 32)]
    public string? ResetTime { get; set; }
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

