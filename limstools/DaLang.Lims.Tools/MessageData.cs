namespace DaLang.Lims.Tools;

public class MessageData<T>
{
    public string Component { get; set; }
    public MessageType Type { get; set; }
    public T Data { get; set; }
}

public class MessageResult
{
    public MessageType Type { get; set; }
    public string Component { get; set; }
    public bool Success { get; set; } = true;
    public string? Msg { get; set; } = string.Empty;
}
public enum MessageType
{
    Print,
    SnapShot,
    ChangeExam,
    Ping = 9,
}
public class PrintDto
{
    public string TemplateName { get; set; }
    public object PrintList { get; set; }
}

public class SnapShotDto
{
    public string AccessToken { get; set; }
    public string SampleNo { get; set; }
    public long ExamInfoId { get; set; }
    public bool IsGrossExamination { get; set; } = false;
}
