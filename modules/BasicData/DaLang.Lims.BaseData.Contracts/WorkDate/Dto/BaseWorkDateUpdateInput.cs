namespace DaLang.Lims.BaseData.Contracts.WorkDate.Dto;

/// <summary>
/// 工作日更新数据输入
/// </summary>
public class BaseWorkDateUpdateInput : BaseWorkDateAddInput
{
    public long Id { get; set; }
}
