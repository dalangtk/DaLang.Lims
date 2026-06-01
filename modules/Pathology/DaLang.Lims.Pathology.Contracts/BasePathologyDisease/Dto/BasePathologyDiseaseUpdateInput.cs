
namespace DaLang.Lims.Pathology.Contracts.BasePathologyDisease.Dto;

/// <summary>
/// 疾病更新数据输入
/// </summary>
public partial class BasePathologyDiseaseUpdateInput : BasePathologyDiseaseAddInput
{
    public long Id { get; set; }
}