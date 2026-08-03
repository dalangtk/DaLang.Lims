
namespace DaLang.Lims.Web.Framework.Services.QuartzTask.Dto;

/// <summary>
/// 定时任务更新数据输入
/// </summary>
public partial class QuartzTaskUpdateInput : QuartzTaskAddInput
{
    public long Id { get; set; }
}