using System.ComponentModel;

namespace DaLang.Lims.Agent.Contracts.Dto;

public class UserInput
{
    public string Query { get; set; }
}
public class TimeInput
{
    [Description("起始时间")]
    public DateTime? BeginTime { get; set; }
    [Description("截止时间")]
    public DateTime? EndTime { get; set; }
}
public class CustomerCodeInput
{
    [Description("客户编码")]
    public string? CustomerCode { get; set; }

}
public class TakeSizeInput
{
    [Description("取多少条数据")]
    public int? Size { get; set; } = 20;
}