
namespace DaLang.Lims.Pathology.Contracts.BasePathologySubDisease.Dto;

/// <summary>
/// 子疾病更新数据输入
/// </summary>
public partial class BasePathologySubDiseaseUpdateInput : BasePathologySubDiseaseAddInput
{
    public long Id { get; set; }
}