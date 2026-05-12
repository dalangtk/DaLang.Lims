namespace DaLang.Lims.Pathology.Contracts.PathologyTemplate.Dto;

/// <summary>
/// 诊断模板更新输入
/// </summary>
public class PathologyTemplateUpdateInput : PathologyTemplateAddInput
{
    /// <summary>
    ///Id
    ///</summary>
    public long Id { get; set; }
}
