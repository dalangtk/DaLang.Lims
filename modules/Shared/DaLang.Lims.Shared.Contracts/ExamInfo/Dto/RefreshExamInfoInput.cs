namespace DaLang.Lims.Shared.Contracts.Dto;

/// <summary>
/// 
/// </summary>
public class RefreshExamInfoInput
{
    public bool IsReceive { get; set; } = false;
    public long ExamInfoId { get; set; }
    public int IsForce { get; set; }
}
