namespace DaLang.Lims.BaseData.Contracts.Sequence.Dto;

/// <summary>
/// 序列新增输入
/// </summary>
public partial class BaseSequenceAddInput
{
    /// <summary>
    /// 序列代码
    /// </summary>
    public string? SequenceCode { get; set; }
    /// <summary>
    /// 序列名称
    /// </summary>
    public string? SequenceName { get; set; }
    /// <summary>
    /// 序列标识
    /// </summary>
    public string? Identifier { get; set; }
    /// <summary>
    /// 前缀
    /// </summary>
    public string? Perfix { get; set; }
    /// <summary>
    /// 序列内容
    /// </summary>
    public string? SequenceValue { get; set; }
    /// <summary>
    /// 起始号
    /// </summary>
    public int? StartNo { get; set; }
    /// <summary>
    /// 步长
    /// </summary>
    public int? SequenceStep { get; set; }
    /// <summary>
    /// 补齐长度
    /// </summary>
    public int? CompletionLength { get; set; }
    /// <summary>
    /// 补齐字符
    /// </summary>
    public string? CompletionChar { get; set; }
    /// <summary>
    /// 后缀
    /// </summary>
    public string? Suffix { get; set; }
    /// <summary>
    /// 重置类型
    /// </summary>
    public int? ResetType { get; set; }
    /// <summary>
    /// 重置时间
    /// </summary>
    public string? ResetTime { get; set; }
    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }
    /// <summary>
    /// 启用
    /// </summary>
    public bool IsValid { get; set; }
}
