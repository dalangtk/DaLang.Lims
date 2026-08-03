
namespace DaLang.Lims.Web.Framework.Services.QuartzTask.Dto;

/// <summary>
/// 定时任务查询输入
/// </summary>
public partial class QuartzTaskQueryInput
{
    public string? TaskName { get; set; }
    public string? GroupName { get; set; }
    public string? TaskOrGroupName { get; set; }
}