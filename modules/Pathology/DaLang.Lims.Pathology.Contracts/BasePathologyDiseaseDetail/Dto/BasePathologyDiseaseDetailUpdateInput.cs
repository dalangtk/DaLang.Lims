
namespace DaLang.Lims.Pathology.Contracts.BasePathologyDiseaseDetail.Dto;

/// <summary>
/// 疾病明细更新数据输入
/// </summary>
public partial class BasePathologyDiseaseDetailUpdateInput : BasePathologyDiseaseDetailAddInput
{
    public long Id { get; set; }
}