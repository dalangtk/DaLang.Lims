using DaLang.Lims.Web.Framework.Core.Dto;

namespace DaLang.Lims.Pretreatment.Contracts.Handover.Dto;

public class GetTaskOutput<T>
{
    /// <summary>
    /// 0选择标本类型
    /// </summary>
    public int Status { get; set; }
    public List<CodeNameDto> SampleTypeList { get; set; }
    public T OutputList { get; set; }
}
