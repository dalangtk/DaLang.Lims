
using System.ComponentModel;

namespace DaLang.Lims.Agent.Contracts.Dto;


[Compose(typeof(TimeInput), typeof(CustomerCodeInput), typeof(TakeSizeInput))]
public partial class CustomerSampleCountInput
{

}
[Compose(typeof(TimeInput), typeof(CustomerCodeInput))]
public partial class CustomerSampleCountOutput
{
    [Description("客户名称")]
    public string? CustomerName { get; set; }
    [Description("检测日期")]
    public DateTime? TestDate { get; set; }
    [Description("数量")]
    public int Count { get; set; }
}
