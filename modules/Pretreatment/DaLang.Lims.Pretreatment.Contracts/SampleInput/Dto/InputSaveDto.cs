namespace DaLang.Lims.Pretreatment.Contracts.SampleInput.Dto;

public class InputSaveDto
{
    /// <summary>
    /// 录入次数
    /// </summary>
    public InputOrderEnum InputOrder { get; set; }
    /// <summary>
    /// 录入类型
    /// </summary>
    public InputTypeEnum InputType { get; set; }
    public InputInfoDto? InputInfo { get; set; }
    public List<InputItemDto>? InputItems { get; set; } = new List<InputItemDto>();
}
public enum InputTypeEnum
{
    Info = 1,
    Item = 2,
    DirectInput = 3
}
public enum InputOrderEnum
{
    First = 1,
    Second = 2
}
