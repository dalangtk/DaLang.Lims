
namespace DaLang.Lims.Web.Framework.Contracts.SysQuartzTaskLog.Dto;

/// <summary>
/// 任务日志更新数据输入
/// </summary>
public partial class QuartzTaskLogUpdateInput : QuartzTaskLogAddInput
{
    public long Id { get; set; }
}