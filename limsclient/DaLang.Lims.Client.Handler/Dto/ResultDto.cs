namespace DaLang.Lims.Client.Handler.Dto;

public class ResultDto
{
    public bool Success { get; set; }
    public string Msg { get; set; } = string.Empty;
    public string Component { get; set; }
}
