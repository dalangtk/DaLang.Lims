namespace DaLang.Lims.BaseData.Contracts.Sequence.Dto;

/// <summary>
/// 序列更新数据输入
/// </summary>
public class BaseSequenceUpdateInput : BaseSequenceAddInput
{
    public long Id { get; set; }
}
